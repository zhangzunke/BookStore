using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Apartments;
using BookStore.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Apartments.UpdateApartments
{
    internal sealed class UpdateApartmentCommandHandler : ICommandHandler<UpdateApartmentCommand>
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateApartmentCommandHandler(
            IApartmentRepository apartmentRepository,
            IUnitOfWork unitOfWork)
        {
            _apartmentRepository = apartmentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateApartmentCommand request, CancellationToken cancellationToken)
        {
            Apartment? apartment = await _apartmentRepository.GetByIdAsync(new ApartmentId(request.ApartmentId), cancellationToken);

            if (apartment is null)
            {
                return Result.Failure(ApartmentErrors.NotFound);
            }

            var priceResult = new Money(
                request.PriceAmount,
                Currency.FromCode(request.PriceAmountCurrency));

            var cleaningFeeResult = new Money(
                request.CleaningFeeAmount,
                Currency.FromCode(request.PriceAmountCurrency));

            apartment.Update(priceResult, cleaningFeeResult, request.Amenities);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
