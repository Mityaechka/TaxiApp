using System.Collections.Generic;

namespace TaxiApp.DependencyServices
{
    public interface IAppListService
    {
        List<string> GetAppsList();
    }
}
