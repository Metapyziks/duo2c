using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUO2C
{
    static class Tools
    {
        public static bool Extends(this Type self, Type t)
        {
            if (self.BaseType == t) return true;
            if (self.BaseType != null) return self.BaseType.Extends(t);
            return false;
        }
    }
}
