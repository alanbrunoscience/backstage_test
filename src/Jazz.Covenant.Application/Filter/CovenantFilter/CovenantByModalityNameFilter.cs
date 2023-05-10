using Jazz.Covenant.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Filter.CovenantFilter
{
    public class CovenantByModalityNameFilter : ICustomFilter
    {
        private string? NameModality;

        public CovenantByModalityNameFilter(string ?nameModality)
        {
            NameModality = nameModality;
        }

        public IDictionary<string, object> GetFilter()
        {
            var whereModalityName = new Dictionary<string, object>();
            whereModalityName.Add("m.Name Like '%",$"{NameModality}%'");
            return whereModalityName;
        }

        public bool Validate()
        {
            return NameModality !=  null;
        }
    }
}
