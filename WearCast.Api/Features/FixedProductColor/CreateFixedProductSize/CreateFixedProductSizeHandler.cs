using WearCast.Api.Entities.FixedProduct;
using WearCast.Api.Features.FixedProductColor.CreateFixedProductSize.DTOs;

namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductSize
{
    public class CreateFixedProductSizeHandler : IRequestHandler<CreateFixedProductSizeRequestDto, bool>
    {
        private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepository;

        private readonly ApplicationDbContext _context;

        public CreateFixedProductSizeHandler(
            IRepository<Entities.FixedProduct.FixedProductColor> colorRepository,
            ApplicationDbContext context) 
        {
            _colorRepository = colorRepository;
            _context = context;
        }

        public async Task<bool> Handle(CreateFixedProductSizeRequestDto request, CancellationToken cancellationToken)
        {
            var color = await _colorRepository.GetAsync(
                c => c.Id == request.ProductColorId,
                useNoTracking: true
            );

            if (color == null)
            {
                return false;
            }

            var newSize = new FixedProductSize
            {
                ProductColorId = request.ProductColorId,
                Size = request.Size,
                Quantity = request.Quantity
            };
            await _context.FixedProductSizes.AddAsync(newSize, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}