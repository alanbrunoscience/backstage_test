using Jazz.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Domain
{
    public class CovenantFavorite:Entity<Guid>
    {
       
        public bool Favorite { get; set; }
        public  string TaxId { get; set; }
        public Covenant Covenant { get; set; }
        public CovenantId CovenantId { get; set; }
     
    }
}
