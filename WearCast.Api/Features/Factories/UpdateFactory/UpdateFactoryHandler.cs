namespace WearCast.Api.Features.Factories.UpdateFactory
{
    public class UpdateFactoryHandler(
         ApplicationDbContext context,
         IHttpContextAccessor httpContextAccessor,
         IMapper mapper
        ) : IRequestHandler<UpdateFactoryRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;
        public async Task<Result> Handle(UpdateFactoryRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetFactoryId;

            if (user.IsSuperAdmin() || user.IsVendorAdmin())
            {
                if (!request.ProvidedFactoryId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetFactoryId = request.ProvidedFactoryId.Value;
            }
            else
            {
                targetFactoryId = user.GetFactoryId()!.Value;
            }

            var factory = await _context.Factories
                .FirstOrDefaultAsync(x => x.Id == targetFactoryId, cancellationToken);

            if (factory == null)
            {
                return Result.Failure(FactoryErrors.FactoryNotFound);
            }

            _mapper.Map(request, factory);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
