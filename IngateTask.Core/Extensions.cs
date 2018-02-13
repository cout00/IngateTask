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

        public static Uri Merge(this Uri self, Uri secondPart)
        {
            try
            {
                var tryextr= self.LocalPath;
            }
            catch (Exception e)
            {
                string mainStr = self.OriginalString.Replace("/", "");
                if (mainStr.Length==0)
                {
                    return secondPart;
                }
                string newStr=String.Empty;
                if (secondPart.OriginalString.Last()=='/')
                {
                    newStr = secondPart.OriginalString + mainStr + '/';
                }
                else
                {
                    newStr = secondPart.OriginalString+'/'+ mainStr + '/';
                }
                return newStr.ToUri();
            }
            return self;
        }

        public static bool IsSubDomain(this Uri self)
        {
            string patternGetDomain = @"(http(|s)):\/\/(.*?)\/";
            string patternIsSubDomain = @"^([A-Za-z0-9](?:(?:[-A-Za-z0-9]){0,61}[A-Za-z0-9])?(?:\.[A-Za-z0-9](?:(?:[-A-Za-z0-9]){0,61}[A-Za-z0-9])?){2,})$";
            Regex regex=new Regex(patternGetDomain);
            string clearDomen= regex.Match(self.OriginalString).Groups[3].Value;
            regex=new Regex(patternIsSubDomain);
            return regex.IsMatch(clearDomen);
        }


        public static Uri ToUri(this string self)
        {
            return new Uri(self,UriKind.RelativeOrAbsolute);
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

        public static IEnumerable<Type> GetAgentsType()
        {
           return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(IUserAgent).IsAssignableFrom(type) && type != typeof(CustomAgent) &&
                               type != typeof(IUserAgent));
        }

    }
}
