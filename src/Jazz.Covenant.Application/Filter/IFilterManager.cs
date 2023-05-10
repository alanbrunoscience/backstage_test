using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Filter
{
    public interface IFilterManager
    {
        public IFilterManager AddFilter(ICustomFilter filter);
        public SqlBuilder Execute();

    }
}
