using Prism.Mvvm;

namespace WireAntennaUI.ViewModels {
    public class MainWindowViewModel : BindableBase {

        private string title = "Wire Antenna Calculator";
        public string Title {
            get => title;
            set => SetProperty(ref title, value);
        }

        public MainWindowViewModel() { }
    }

#if DEBUG
    public class MainWindowViewModelDesign : MainWindowViewModel {
        public MainWindowViewModelDesign() {
            Title = "Wire Antenna Calculator";
        }
    }
#endif
}
