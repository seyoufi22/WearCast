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

        var value = valueProviderResult.FirstValue;

        // Check if the value is null or empty
        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        try
        {
            // Deserialize the JSON string to the target type
            var result = JsonSerializer.Deserialize(value, bindingContext.ModelType, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (JsonException)
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid JSON format");
        }

        return Task.CompletedTask;
    }
}
