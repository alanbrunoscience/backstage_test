using Jazz.Covenant.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Filter.CovenantFavoriteFilter
{
    public class CovenantFavoriteByTaxId : ICustomFilter
    {
        private string TaxId;

        public CovenantFavoriteByTaxId(string taxId)
        {
            TaxId = taxId;
        }

        public IDictionary<string, object> GetFilter()
        {
            var whereTaxId = new Dictionary<string, object>();
            whereTaxId.Add("TaxId =", $"'{TaxId}'");
            return whereTaxId;
        }

        public bool Validate()
        {
            return TaxId != String.Empty;
        }
    }
}
