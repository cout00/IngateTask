using System.Collections.Generic;

namespace IngateTask.Core.Interfaces
{
/// <summary>
/// контракт для тестирования парсера робот.тхт. в тестах заменен на стаб 
/// </summary>
    public interface IRequest
    {       
        List<string> GetFileFromDomain(string uri);
    }
}