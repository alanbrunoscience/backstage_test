using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Filter.CovenantFilter
{
    public class CovenantByOrganizationPublicFilter : ICustomFilter
    {
        private string? OrganizationPublic;

        public CovenantByOrganizationPublicFilter(string ?organizationPublic)
        {
            OrganizationPublic = organizationPublic;
        }

        public IDictionary<string, object> GetFilter()
        {
            var whereOrganizationPublic = new Dictionary<string, object>();
            whereOrganizationPublic.Add("Covenants.Organization Like '%", $"{OrganizationPublic}%'");
            return whereOrganizationPublic;
        }

        public bool Validate()
        {
            return OrganizationPublic != null;
        }
    }
}
