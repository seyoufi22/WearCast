using WearCast.Api.Features.Sellers.SellerApplications.GetSellerApplicationById.DTOs;
using WearCast.Api.Features.Shipments;
using WearCast.Api.Features.Shipments.AdminAndManager.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Sellers.SellerApplications.GetSellerApplicationById.Handlers
{
    public class GetSellerApplicationByIdHandler : IRequestHandler<GetSellerApplicationByIdRequestDTO, Result<GetSellerApplicationByIdResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetSellerApplicationByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetSellerApplicationByIdResponseDTO>> Handle(
            GetSellerApplicationByIdRequestDTO request,
            CancellationToken cancellationToken)
        {
            var application = await _context.SellerApplications
                .AsNoTracking()
                .Where(a => a.Id == request.ApplicationId)
                .Select(a => new GetSellerApplicationByIdResponseDTO
                {
                    Id = a.Id,

                    ManagerFirstName = a.ManagerFirstName,
                    ManagerLastName = a.ManagerLastName,
                    ManagerEmail = a.ManagerEmail,
                    ManagerPhoneNumber = a.ManagerPhoneNumber,
                    ManagerEmailConfirmed = a.ManagerEmailConfirmed,

                    SellerName = a.SellerName,
                    SellerEmail = a.SellerEmail,
                    SellerPhoneNumber = a.SellerPhoneNumber,
                    CommercialRegisterNumber = a.CommercialRegisterNumber,
                    TaxIdNumber = a.TaxIdNumber,
                    Description = a.Description,
                    LogoUrl = a.LogoUrl,
                    SellerAddress = a.SellerAddress,

                    CreatedOn = a.CreatedOn,
                    Status = a.Status,
                    RejectionReason = a.RejectionReason
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (application == null)
            {
                return Result.Failure<GetSellerApplicationByIdResponseDTO>(SellerApplicationErrors.ApplicationNotFound);
            }

            return Result.Success(application);
        }
    }
}
