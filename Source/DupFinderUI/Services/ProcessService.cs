using System.Diagnostics;

namespace DupFinderUI.Services
{
    public class ProcessService
    {
        public void StartProcess(string process) => Process.Start(process);
    }
}
