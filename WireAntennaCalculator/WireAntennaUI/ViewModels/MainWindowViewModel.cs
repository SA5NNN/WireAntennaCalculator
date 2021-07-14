using System;
using System.Diagnostics;
using Prism.Mvvm;
using Windows.ApplicationModel;

namespace WireAntennaUI.ViewModels {
    public class MainWindowViewModel : BindableBase {

        public MainWindowViewModel() {
            var version = GetVersion();
            Title = $"Wire Antenna Calculator - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private string title;
        public string Title {
            get => title;
            set => SetProperty(ref title, value);
        }

        [DebuggerStepThrough]
        public PackageVersion GetVersion() {
            try {
                var package = Package.Current;
                return package.Id.Version;
            } catch (InvalidOperationException) {
                return new PackageVersion();
            }
        }
    }

#if DEBUG
    public class MainWindowViewModelDesign : MainWindowViewModel {
    }
#endif
}
