using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Filter.CovenantFilter
{
    public class CovenatByActive : ICustomFilter
    {
        private int? Active;

        public CovenatByActive(bool ?active)
        {  
            Active = active==null?null:Convert.ToInt32(active);
        }

        public IDictionary<string, object> GetFilter()
        {
            var whereActive = new Dictionary<string, object>();
            whereActive.Add("Covenants.Active = ", $"{Active}");
            return whereActive;
        }
        public bool Validate()
        {
            return Active != null;
        }
    }
}
