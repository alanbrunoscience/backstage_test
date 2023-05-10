using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Filter.CovenantFavoriteFilter
{
    public class CovenantFavoriteByConvenantId : ICustomFilter
    {
        private Guid CovenantId;
        public CovenantFavoriteByConvenantId(Guid covenantId)
        {
            CovenantId = covenantId;
        }

        public IDictionary<string, object> GetFilter()
        {
            var whereCovenantId = new Dictionary<string, object>();
            whereCovenantId.Add("CovenantId =", $"'{CovenantId}'");
            return whereCovenantId;

        }

        public bool Validate()
        {
            return CovenantId != Guid.Empty;
        }
    }
}
