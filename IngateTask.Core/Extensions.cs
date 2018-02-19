using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IngateTask.PortableLibrary.UserAgents;

namespace IngateTask.Core
{
    /// <summary>
    /// расширения
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// монада ин. просто удобство
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="mathes"></param>
        /// <returns></returns>
        public static bool In<T>(this T self, params T[] mathes)
        {
            return mathes.Contains(self);
        }
        /// <summary>
        /// монада сабстринг оф. ищет подстроку в массиве строк
        /// </summary>
        /// <param name="self"></param>
        /// <param name="mathes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// унификация всех ури. все ури без последнего слеша
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        private static string RemoveSlash(string st)
        {
            if (st.EndsWith("/"))
            {
                return st.Remove(st.Length - 1);
            }
            return st;
        }

        /// <summary>
        /// ищет анкоры и редиректы. можно было и регуляркой
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool UriIsAnchorOrRedirect(this Uri self)
        {
            return self.OriginalString.Contains("#") || self.OriginalString.Contains("?url=") ||
                   self.OriginalString.Contains("&url=") || self.OriginalString.Contains("?");
        }

        /// <summary>
        /// составляет из ури новый ури. как комбинации абсолютных или относительных путей
        /// т.е ури начинающиеся на / ./ или ../
        /// </summary>
        /// <param name="selfUri"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Uri CombinePath(this Uri selfUri, string path)
        {
            string uri = selfUri.OriginalString;
            uri = RemoveSlash(uri);
            if (path.StartsWith("/"))
            {
                return (RemoveSlash(selfUri.GetBaseAdress()) + RemoveSlash(path)).ToUri();
            }
            if (path.StartsWith("./"))
            {
                path = path.TrimStart('.');
                return (uri + RemoveSlash(path)).ToUri();
            }
            return selfUri;
        }
        
        /// <summary>
        /// базвый адрес это просто вычмтание абсолютного адреса и пути
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string GetBaseAdress(this Uri self)
        {
            if (!self.AbsolutePath.In("/", ""))
            {
                return RemoveSlash(self.AbsoluteUri.Replace(self.AbsolutePath, ""));
            }
            return RemoveSlash(self.OriginalString);
        }

        /// <summary>
        /// соединяет два ури
        /// </summary>
        /// <param name="self"></param>
        /// <param name="secondPart"></param>
        /// <returns></returns>
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

        /// <summary>
        /// сахар
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Uri ToUri(this string self)
        {
            return new Uri(RemoveSlash(self), UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// сахар
        /// </summary>
        /// <param name="self"></param>
        /// <param name="comparentUri"></param>
        /// <returns></returns>
        public static bool UriHaveSameDomens(this Uri self, Uri comparentUri)
        {
            return string.Compare(self.Host, comparentUri.Host) == 0;
        }

        
/// <summary>
/// преобразует ури в название файла убирая все запрещенные символы
/// так же если прям совсем длинный заменяем на рандомное имя
/// нам не нужен дроп на переполнении размера пути
/// </summary>
/// <param name="self"></param>
/// <returns></returns>
        public static string ToFilePath(this Uri self)
        {
            string endstring = self.LocalPath
                                   .Replace("\\", "_")
                                   .Replace("/", "_")
                                   .Replace(":", "_")
                                   .Replace("?", "_")
                                   .Replace("*", "_")
                                   .Replace("\"", "_")
                                   .Replace("<", "_")
                                   .Replace(">", "_")
                                   .Replace("|", "_") + ".txt";
            if (endstring.Length >= 50)
            {
                endstring = Path.GetRandomFileName() + ".txt";
            }
            return endstring;
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