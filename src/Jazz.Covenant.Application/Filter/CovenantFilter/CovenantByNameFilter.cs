using Jazz.Covenant.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Filter.CovenantFilter
{
    public class CovenantByNameFilter : ICustomFilter
    {
        private string? Name;

        public CovenantByNameFilter(string? name)
        {
            Name = name;
        }

        public IDictionary<string, object> GetFilter()
        {
            var whereName = new Dictionary<string, object>();
            whereName.Add("Covenants.Name Like '%",$"{Name}%'");
            return whereName;
        }

        public bool Validate()
        {
            return Name != null;
        }
    }
}
