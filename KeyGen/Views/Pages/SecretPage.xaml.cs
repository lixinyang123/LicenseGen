using Wpf.Ui.Common.Interfaces;

namespace KeyGen.Views.Pages
{
    /// <summary>
    /// SecretPage.xaml 的交互逻辑
    /// </summary>
    public partial class SecretPage : INavigableView<ViewModels.SecretPageViewModel>
    {
        public ViewModels.SecretPageViewModel ViewModel
        {
            get;
        }

        public SecretPage(ViewModels.SecretPageViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
