using System.Diagnostics;

namespace AppTimerService.Contracts.Utils
{
    public interface IProcessHelper
    {
        Process GetForegroundProcess();
    }
}
