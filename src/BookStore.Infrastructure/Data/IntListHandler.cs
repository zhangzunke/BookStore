using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Data
{
    internal sealed class IntListHandler : SqlMapper.TypeHandler<List<int>>
    {
        public override List<int>? Parse(object value)
        {
            if (value == DBNull.Value || value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return [];
            }
            var strValue = value.ToString();
            return strValue!.Split(',').Select(int.Parse).ToList();
        }

        public override void SetValue(IDbDataParameter parameter, List<int>? value)
        {
            if (value == null)
                parameter.Value = string.Empty;
            else
                parameter.Value = string.Join(",", value);
        }
    }
}
