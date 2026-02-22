namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.GetSellerApplicationByEmail
{
    public class GetSellerApplicationHandler(
        ApplicationDbContext context,
        IMapper mapper
        ) : IRequestHandler<GetSellerApplicationRequest, Result<SellerApplicationResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<SellerApplicationResponse>> Handle(GetSellerApplicationRequest request, CancellationToken cancellationToken)
        {
            var application = await _context.SellerApplications
             .Include(x => x.StoreAddress)
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Email == request.Email && x.Status == Status.Pending, cancellationToken);

            if (application == null)
                return Result.Failure<SellerApplicationResponse>(SellerApplicationErrors.ApplicationNotFound);

            var response = _mapper.Map<SellerApplicationResponse>(application);

            return Result.Success(response);
        }
    }
}
