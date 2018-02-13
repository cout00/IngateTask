using System;
using System.Collections.Generic;

namespace IngateTask.Core.Interfaces
{
    public interface IHttpParser
    {
        IEnumerable<Uri> GetNestedUri(string page, Uri baseUri);
    }
}