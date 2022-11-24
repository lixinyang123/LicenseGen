using CommunityToolkit.Mvvm.Input;
using KeyGen.Models;
using KeyGen.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Wpf.Ui.Controls;

namespace KeyGen.ViewModels
{
    public partial class DashboardViewModel
    {
        private readonly string deviceList = "Assets/Devices.json";

        private readonly SecurityService securityService;

        public bool DevToolEnabled { get; set; }

        public bool CalibrationEnabled { get; set; }

        public string SelectedLeftDevice { get; set; } = string.Empty;

        public ObservableCollection<string> LeftDevices { get; set; }

        public string SelectedRightDevice { get; set; } = string.Empty;

        public ObservableCollection<string> RightDevices { get; set; } = new();

        public DashboardViewModel(SecurityService securityService)
        {
            this.securityService = securityService;
            LeftDevices = JsonSerializer.Deserialize<ObservableCollection<string>>(File.ReadAllText(deviceList)) ?? new();
        }

        [RelayCommand]
        private void MoveRight()
        {
            if (string.IsNullOrEmpty(SelectedLeftDevice))
            {
                return;
            }

            RightDevices.Add(SelectedLeftDevice);
            _ = LeftDevices.Remove(SelectedLeftDevice);
        }

        [RelayCommand]
        private void MoveLeft()
        {
            if (string.IsNullOrEmpty(SelectedRightDevice))
            {
                return;
            }

            LeftDevices.Add(SelectedRightDevice);
            _ = RightDevices.Remove(SelectedRightDevice);
        }

        [RelayCommand]
        private void Generate()
        {
            TokenInfo tokenInfo = new()
            {
                DevToolEnabled = DevToolEnabled,
                CalibrationEnabled = CalibrationEnabled,
                Devices = RightDevices.ToList()
            };

            //var result = securityService.Encrypt(JsonSerializer.Serialize(tokenInfo));
            //string temp = securityService.Decrypt(result);

            securityService.SignData(JsonSerializer.Serialize(tokenInfo), out string data, out string signature);
            bool flag = securityService.VerifyData(data, signature);
        }
    }
}
