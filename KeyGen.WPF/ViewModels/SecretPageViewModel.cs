using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeyGen.Services;

namespace KeyGen.ViewModels
{
    public partial class SecretPageViewModel : ObservableObject
    {
        private readonly SecurityService securityService;

        [ObservableProperty]
        public string privateKey = string.Empty;

        [ObservableProperty]
        public string publicKey = string.Empty;

        public SecretPageViewModel(SecurityService securityService) 
        {
            this.securityService = securityService;
            Init();
        }

        private void Init()
        {
            PrivateKey = securityService.PrivateKey;
            PublicKey = securityService.PublicKey;
        }

        [RelayCommand]
        public void GenerateSecret()
        {
            securityService.Generate(true);
            Init();
        }
    }
}
