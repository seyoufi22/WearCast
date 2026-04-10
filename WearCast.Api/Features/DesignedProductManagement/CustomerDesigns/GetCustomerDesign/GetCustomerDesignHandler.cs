namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetCustomerDesign
{
    public class GetCustomerDesignHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ILogger<GetCustomerDesignHandler> logger
        ) : IRequestHandler<GetCustomerDesignRequest, Result<GetCustomerDesignResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<GetCustomerDesignHandler> _logger = logger;

        public async Task<Result<GetCustomerDesignResponse>> Handle(GetCustomerDesignRequest request, CancellationToken cancellationToken)
        {
            var customerId = _httpContextAccessor.HttpContext?.User?.GetCustomerId();
            if (customerId == null) return Result.Failure<GetCustomerDesignResponse>(AuthErrors.Forbidden);

            try
            {
                var design = await _context.CustomerDesigns
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d =>
                        d.Id == request.Id &&
                        d.CustomerId == customerId.Value,
                    cancellationToken);

                if (design == null)
                {
                    return Result.Failure<GetCustomerDesignResponse>(CustomerDesignErrors.DesignNotFound);
                }


                var response = new GetCustomerDesignResponse(
                    design.Id,
                    design.DesignedProductId,
                    design.DesignedProductColorId,
                    design.ViewDesignsJson,
                    design.FrontImageUrl,
                    design.BackImageUrl,
                    design.RightImageUrl,
                    design.LeftImageUrl
                );

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve design details for DesignId: {DesignId}", request.Id);

                return Result.Failure<GetCustomerDesignResponse>(
                    new Error("CustomerDesign.FetchFailed", "An error occurred while retrieving the design details.", StatusCodes.Status500InternalServerError));
            }
        }
    }
}
