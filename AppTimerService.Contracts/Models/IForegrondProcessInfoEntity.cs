using System.Diagnostics;

namespace AppTimerService.Contracts.Models
{
    public interface IForegroundProcessInfoEntity
    {
        int Id { get; set; }
        string ProcessName { get; set; }
        string ForegroundDuration { get; set; }
        void Initialize(Process process);
    }
}
