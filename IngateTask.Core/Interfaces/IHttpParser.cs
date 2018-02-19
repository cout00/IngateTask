using System;
using System.Collections.Generic;

namespace IngateTask.Core.Interfaces
{
    /// <summary>
    /// контракт на способ разбора страницы
    /// </summary>
    public interface IHttpParser
    {
        IEnumerable<Uri> GetNestedUri(string page, Uri baseUri);
    }
}