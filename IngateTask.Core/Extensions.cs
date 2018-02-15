using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IngateTask.Core.Interfaces;
using IngateTask.Core.UserAgents;

namespace IngateTask.Core
{
    public static class Extensions
    {
        public static bool In<T>(this T self, params T[] mathes)
        {
            return mathes.Contains(self);
        }
        public static bool IsSubStringOf(this string self, params string[] mathes)
        {
            foreach (string str in mathes)
            {
                if (self.Contains(str))
                {
                    return true;
                }
            }
            return false;
        }

        static string AddSlash(string st)
        {
            if (!st.EndsWith("/"))
            {
                return st += "/";
            }
            return st;
        }

        static string RemoveSlash(string st)
        {
            if (st.EndsWith("/"))
            {
                return st.Remove(st.Length - 1);
            }
            return st;
        }

        public static bool UriIsAnchorOrRedirect(this Uri self)
        {
            return self.OriginalString.Contains("#")|| self.OriginalString.Contains("?url=")|| self.OriginalString.Contains("&url=");
        }


        public static Uri CombinePath(this Uri selfUri, string path)
        {
            var uri = selfUri.OriginalString;
            uri = RemoveSlash(uri);
            if (path.StartsWith("/"))
            {
                return (RemoveSlash(selfUri.GetBaseAdress()) + AddSlash(path)).ToUri();
            }
            if (path.StartsWith("./"))
            {
                path = path.TrimStart('.');
                return (uri + AddSlash(path)).ToUri();
            }
            return selfUri;
        }

        public static string GetBaseAdress(this Uri self)
        {            
            if (!self.AbsolutePath.In("/",""))
            {
                return AddSlash((self.AbsoluteUri.Replace(self.AbsolutePath, "")));
            }
            return AddSlash(self.OriginalString);
        }


        public static Uri Merge(this Uri self, Uri secondPart)
        {
            try
            {
                var tryextr = self.LocalPath;
            }
            catch (Exception e)
            {
                if (secondPart.OriginalString.Contains(self.OriginalString))
                {
                    return secondPart;
                }
                return secondPart.CombinePath(self.OriginalString);
            }
            return self;
        }

        public static bool IsSubDomain(this Uri self)
        {
            string patternGetDomain = @"(http(|s)):\/\/(.*?)\/";
            string patternIsSubDomain = @"^([A-Za-z0-9](?:(?:[-A-Za-z0-9]){0,61}[A-Za-z0-9])?(?:\.[A-Za-z0-9](?:(?:[-A-Za-z0-9]){0,61}[A-Za-z0-9])?){2,})$";
            Regex regex = new Regex(patternGetDomain);
            string clearDomen = regex.Match(self.OriginalString).Groups[3].Value;
            regex = new Regex(patternIsSubDomain);
            return regex.IsMatch(clearDomen);
        }


        public static Uri ToUri(this string self)
        {
            //self = AddSlash(self);
            return new Uri(self, UriKind.RelativeOrAbsolute);
        }

        public static bool UriHaveSameDomens(this Uri self, Uri comparentUri)
        {
            return string.Compare(self.Host, comparentUri.Host) == 0;
        }

        public static string ToFilePath(this Uri self)
        {
            return self.LocalPath
                .Replace("\\", "_")
                .Replace("/", "_")
                .Replace(":", "_")
                .Replace("?", "_")
                .Replace("*", "_")
                .Replace("\"", "_")
                .Replace("<", "_")
                .Replace(">", "_")
                .Replace("|", "_");
        }

        public static IEnumerable<Type> GetAssignedType(this Type self, Func<Type,bool> matchPredicate)
        {
            return Assembly.GetAssembly(self)
                .GetTypes()
                .Where(type => self.IsAssignableFrom(self) && matchPredicate(type));
        } 

        public static IEnumerable<Type> GetAgentsType()
        {
            return Assembly.GetExecutingAssembly()
                 .GetTypes()
                 .Where(type => typeof(IUserAgent).IsAssignableFrom(type) && type != typeof(CustomAgent) &&
                                type != typeof(IUserAgent));
        }

    }
}
