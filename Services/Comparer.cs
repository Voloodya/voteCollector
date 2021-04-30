using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using voteCollector.Models;

namespace voteCollector.Services
{
    public class Comparer : System.Collections.Generic.IEqualityComparer<Street>
    {
        public bool Equals(Street x, Street y)
        {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(Street obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
