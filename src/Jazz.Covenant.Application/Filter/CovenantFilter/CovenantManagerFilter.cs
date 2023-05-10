using Dapper;
using LinqKit;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Filter.CovenantFilter
{
    public class CovenantManagerFilter : IFilterManager
    {
        private Dictionary<string, object> _filters=new Dictionary<string, object>();
        public IFilterManager AddFilter(ICustomFilter filter)
        {
            if (filter.Validate()) 
                filter.GetFilter().ForEach(e=>_filters.Add(e.Key,e.Value));
            return this;
        }
        public SqlBuilder Execute()
        {
            if (_filters.IsNullOrEmpty()) 
                return new SqlBuilder();
            var query = new SqlBuilder();
            _filters.ForEach(e => query.Where($"{e.Key}{e.Value}"));
            return query;
        }
    }
}
