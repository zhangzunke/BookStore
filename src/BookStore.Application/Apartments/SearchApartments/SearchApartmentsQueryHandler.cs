﻿using BookStore.Application.Abstractions.Data;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Bookings;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Apartments.SearchApartments
{
    internal sealed class SearchApartmentsQueryHandler
        : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
    {
        private static readonly int[] ActiveBookingStatuses =
        {
            (int)BookingStatus.Reserved,
            (int)BookingStatus.Confirmed,
            (int)BookingStatus.Completed
        };
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public SearchApartmentsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate > request.EndDate)
                return new List<ApartmentResponse>();

            using var connection = _sqlConnectionFactory.CreateConnection();
            const string sql = """
                SELECT 
                  a.Id,
                  a.Name,
                  a.Description,
                  a.Price_Amount AS Price,
                  a.Price_Currency AS Currency,
                  a.Address_Country AS Country,
                  a.Address_State AS State,
                  a.Address_ZipCode AS ZipCode,
                  a.Address_City AS City,
                  a.Address_Street AS Street
                FROM Apartments AS a
                WHERE NOT EXISTS
                (
                    SELECT 1 
                    FROM Bookings AS b
                    WHERE 
                        b.ApartmentId = a.Id AND
                        b.Duration_Start <= @EndDate AND
                        b.Duration_End >= @StartDate AND
                        b.Status IN @ActiveBookingStatuses
                )
                """;
            var apartments = await connection.QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
                sql,
                (apartment, address) => 
                {
                    apartment.Address = address;
                    return apartment;
                },
                new
                {
                    request.StartDate,
                    request.EndDate,
                    ActiveBookingStatuses
                },
                splitOn: "Country");

            return apartments.ToList();
        }
    }
}
