namespace WearCast.Api.Features.Drivers.UpdateDriver
{
    public class UpdateDriverHandler(
         ApplicationDbContext context,
         IHttpContextAccessor httpContextAccessor,
         IMapper mapper
        ) : IRequestHandler<UpdateDriverRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;
        public async Task<Result> Handle(UpdateDriverRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetDriverId;

            if (user.IsSuperAdmin())
            {
                if (!request.ProvidedDriverId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetDriverId = request.ProvidedDriverId.Value;
            }
            else
            {
                targetDriverId = user.GetDriverId()!.Value;
            }

            var driver = await _context.Drivers
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == targetDriverId, cancellationToken);

            if (driver == null || driver.ApplicationUser == null)
            {
                return Result.Failure(DriverErrors.NotFound);
            }

            var isNationalIdTaken = await _context.Drivers
                .AnyAsync(x => x.NationalId == request.NationalId && x.Id != targetDriverId, cancellationToken);

            if (isNationalIdTaken)
            {
                return Result.Failure(DriverErrors.DuplicatedNationalId);
            }

            _mapper.Map(request, driver);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
