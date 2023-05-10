using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Filter
{
    public interface ICustomFilter
    {
        bool Validate();
        IDictionary<string,object> GetFilter();
    }
}
