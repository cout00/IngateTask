using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using IngateTask.Core.Interfaces;

namespace IngateTask.Core.Parsers
{
    public class RegularExpressionHttpParser :IHttpParser
    {
        public RegularExpressionHttpParser()
        {

        }
        public IEnumerable<Uri> GetNestedUri(string page, Uri baseUri)
        {
            Regex regex = new Regex(@"<a\s+(?:[^>]*?\s+)?href=([""'])(.*?)\1");
            MatchCollection collection = regex.Matches(page);
            for (int i = 0; i < collection.Count; i++)
            {
                if ( !MimeMapping.GetMimeMapping(collection[i].Groups[2].Value)
                    .In(DefaultParams.MemeNonTextList.ToArray()))
                {
                    
                    var link = collection[i].Groups[2].Value.ToUri().Merge(baseUri);
                    yield return link;
                }
            }
        }
    }
}