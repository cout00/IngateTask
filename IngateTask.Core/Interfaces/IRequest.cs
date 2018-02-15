using System.Collections.Generic;

namespace IngateTask.Core.Interfaces
{
    public interface IRequest
    {
        List<string> GetFileFromDomain(string uri);
    }
}