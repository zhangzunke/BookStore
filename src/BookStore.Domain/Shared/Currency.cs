using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Shared
{
    public record Currency
    {
        internal static readonly Currency None = new("");
        public static readonly Currency Usd = new("USD");
        public static readonly Currency Eur = new("EUR");
        private Currency(string code) => Code = code;
        public string Code { get; init; }
        public static Currency FromCode(string code)
            => All.FirstOrDefault(c => c.Code == code) ?? throw new ApplicationException("The currency code is invalid");
        public static readonly IReadOnlyCollection<Currency> All = [Usd, Eur];
    }
}
