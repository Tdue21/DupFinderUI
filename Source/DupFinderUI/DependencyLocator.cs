using DupFinderUI.Models;
using DupFinderUI.Services;
using DupFinderUI.ViewModels;

namespace DupFinderUI
{
    public class DependencyLocator
    {
        private FileSystemService SystemService { get; } = new FileSystemService();
        private ProcessService Service { get; } = new ProcessService();
        private SettingsModel SettingsModel => new SettingsModel(SystemService);
        private DupFinderModel DupFinderModel => new DupFinderModel(SystemService, Service);

        /// <summary>
        /// Gets the main view model.
        /// </summary>
        /// <value>
        /// The main view model.
        /// </value>
        public MainViewModel MainViewModel => new MainViewModel(SettingsModel, DupFinderModel);
    }
}
