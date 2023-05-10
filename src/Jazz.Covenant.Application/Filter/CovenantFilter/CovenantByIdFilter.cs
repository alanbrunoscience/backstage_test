using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Filter.CovenantFilter
{
    public class CovenantByIdFilter : ICustomFilter
    {
        private Guid CovenantId;

        public CovenantByIdFilter(Guid covenantId)
        {
            CovenantId = covenantId;
        }

        public IDictionary<string, object> GetFilter()
        {
            var whereCovenantId=new Dictionary<string, object>();
            whereCovenantId.Add("Covenants.Id =", $"'{CovenantId}'");
            return whereCovenantId;
        }

        public bool Validate()
        {
            return CovenantId != Guid.Empty;
        }
    }
}
