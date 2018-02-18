using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IngateTask.PortableLibrary.UserAgents;

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

        private static string AddSlash(string st)
        {
            if (!st.EndsWith("/"))
            {
                return st += "/";
            }
            return st;
        }

        private static string RemoveSlash(string st)
        {
            if (st.EndsWith("/"))
            {
                return st.Remove(st.Length - 1);
            }
            return st;
        }

        public static bool UriIsAnchorOrRedirect(this Uri self)
        {
            return self.OriginalString.Contains("#") || self.OriginalString.Contains("?url=") ||
                   self.OriginalString.Contains("&url=");
        }


        public static Uri CombinePath(this Uri selfUri, string path)
        {
            string uri = selfUri.OriginalString;
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
            if (!self.AbsolutePath.In("/", ""))
            {
                return AddSlash(self.AbsoluteUri.Replace(self.AbsolutePath, ""));
            }
            return AddSlash(self.OriginalString);
        }


        public static Uri Merge(this Uri self, Uri secondPart)
        {
            try
            {
                string tryextr = self.LocalPath;
            }
            catch (Exception)
            {
                if (secondPart.OriginalString.Contains(self.OriginalString))
                {
                    return secondPart;
                }
                return secondPart.CombinePath(self.OriginalString);
            }
            return self;
        }

        public static Uri ToUri(this string self)
        {
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
                       .Replace("|", "_") + ".txt";
        }


        public static IEnumerable<Type> GetAgentsType()
        {
            return Assembly.GetAssembly(typeof(IUserAgent))
                .GetTypes()
                .Where(type => typeof(IUserAgent).IsAssignableFrom(type) && type != typeof(CustomAgent) &&
                               type != typeof(IUserAgent));
        }
    }
}