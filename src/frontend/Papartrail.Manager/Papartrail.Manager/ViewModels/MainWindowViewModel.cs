using Prism.Mvvm;

namespace Papartrail.Manager.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Xinの博客管理";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
