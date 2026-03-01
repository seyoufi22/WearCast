//namespace WearCast.Api.Features.Sellers.SellerApplications.GetAllSellerApplications
//{
//    public class GetSellerApplicationHandler(
//        ApplicationDbContext context,
//        IMapper mapper
//        ) : IRequestHandler<GetSellerApplicationRequest, Result<IEnumerable<SellerApplicationResponse>>>
//    {
//        private readonly ApplicationDbContext _context = context;
//        private readonly IMapper _mapper = mapper;

//        public async Task<Result<IEnumerable<SellerApplicationResponse>>> Handle(GetSellerApplicationRequest request, CancellationToken cancellationToken)
//        {
//            var applications = await _context.SellerApplications
//             .Where(x => x.Status == Status.Pending)
//             .Include(x => x.StoreAddress)
//             .AsNoTracking()
//             .ToListAsync(cancellationToken);

//            var response = _mapper.Map<IEnumerable<SellerApplicationResponse>>(applications);
//            return Result.Success(response);
//        }
//    }
//}
