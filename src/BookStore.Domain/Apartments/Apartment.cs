using BookStore.Domain.Abstractions;
using BookStore.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Apartments
{
    public sealed class Apartment : Entity<ApartmentId>
    {
        public Apartment(
            ApartmentId id,
            Name name,
            Description description,
            Address address,
            Money price,
            Money cleaningFee,
            List<Amenity> amenities
            ): base(id) 
        {
            Name = name;
            Description = description;
            Address = address;
            Price = price;
            CleaningFee = cleaningFee;
            Amenities = amenities;
        }

        private Apartment()
        {
        }

        public Name Name { get; private set; }
        public Description Description { get; private set; }
        public Address Address { get; private set; }
        public Money Price { get; private set; }
        public Money CleaningFee { get; private set; }
        public DateTime? LastBookedOnUtc { get; internal set; }
        public List<Amenity> Amenities { get; private set; } = [];

        public static Result<Apartment> Create(
            Name name, 
            Description description, 
            Address address, 
            Money price, 
            Money cleaningFee, 
            Amenity[] amenities)
        {
            var apartment = new Apartment(
                ApartmentId.New(),
                name,
                description,
                address,
                price,
                cleaningFee,
                amenities.ToList());

            return Result.Success(apartment);
        }

        public void Update(
            Money priceAmount,
            Money cleaningFeeAmount,
            Amenity[] amenities)
        {
            Price = priceAmount;
            CleaningFee = cleaningFeeAmount;
            Amenities = amenities.ToList();
        }
    }
}
