namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductColor.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

public class JsonModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

        // If the valueProviderResult contains multiple items (like when adding multiple objects in Swagger),
        // we need to combine them into a JSON array string before deserializing.
        var values = valueProviderResult.Values.ToArray();
        string jsonToParse;

        if (values.Length == 1 && (values[0]!.TrimStart().StartsWith("[") || !bindingContext.ModelType.IsGenericType))
        {
            // It's already a JSON array or a single object binding
            jsonToParse = values[0]!;
        }
        else
        {
            // We have multiple individual JSON objects (from Swagger "Add item") or 
            // a single JSON object that needs to be bound to a List<T>.
            // Wrap them in a JSON array.
            jsonToParse = $"[{string.Join(",", values)}]";
        }

        if (string.IsNullOrWhiteSpace(jsonToParse))
        {
            return Task.CompletedTask;
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };

            // Deserialize the JSON string to the target type
            var result = JsonSerializer.Deserialize(jsonToParse, bindingContext.ModelType, options);

            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (JsonException ex)
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"Invalid JSON format: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}
