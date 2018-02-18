using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IngateTask.PortableLibrary
{
    public static class Extensions
    {
        public static IEnumerable<Type> GetAssignedType(this Type self, Func<Type, bool> matchPredicate)
        {
            return Assembly.GetAssembly(self)
                .GetTypes()
                .Where(type => self.IsAssignableFrom(self) && matchPredicate(type));
        }
    }
}