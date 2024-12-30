using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Apartments;
using BookStore.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Apartments.AddApartments
{
    internal sealed class AddApartmentCommandHandler : ICommandHandler<AddApartmentCommand>
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddApartmentCommandHandler(
            IApartmentRepository apartmentRepository,
            IUnitOfWork unitOfWork)
        {
            _apartmentRepository = apartmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddApartmentCommand request, CancellationToken cancellationToken)
        {
            var addressResult = new Address(
                request.Country, 
                request.State, 
                request.ZipCode, 
                request.City, 
                request.Street);

            var priceResult = new Money(request.PriceAmount, Currency.FromCode(request.PriceCurrency));

            var cleaningFeeResult = new Money(request.CleaningFeeAmount, Currency.FromCode(request.CleaningFeeCurrency));

            var apartmentResult = Apartment.Create(
                new Name(request.Name),
                new Description(request.Description),
                addressResult,
                priceResult,
                cleaningFeeResult,
                request.Amenities
                );

            if (apartmentResult.IsFailure) 
            {
                return Result.Failure(apartmentResult.Error);
            }

            _apartmentRepository.Add(apartmentResult.Value);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();

        }
    }
}
