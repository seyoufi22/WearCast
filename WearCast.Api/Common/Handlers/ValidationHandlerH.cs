namespace WearCast.Api.Common.Handlers
{
    public class ValidationHandler<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!validators.Any())
                return await next(cancellationToken);

            var context = new ValidationContext<TRequest>(request);

            var results = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = results
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count == 0)
                return await next(cancellationToken);

            throw new ValidationException(failures);
        }
    }
}
