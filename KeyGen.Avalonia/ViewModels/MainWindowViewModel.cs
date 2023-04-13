using AvaloniaKeyGen.Models;
using AvaloniaKeyGen.Services;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;

namespace AvaloniaKeyGen.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly string deviceList = "Assets/Devices.json";

        private readonly SecurityService securityService;

        public string SelectedLeftDevice { get; set; } = string.Empty;

        public ObservableCollection<string> LeftDevices { get; set; }

        public string SelectedRightDevice { get; set; } = string.Empty;

        public ObservableCollection<string> RightDevices { get; set; } = new();

        public DateTimeOffset Expires { get; set; } = new DateTimeOffset(DateTime.Now.AddDays(30));

        public bool DevToolEnabled { get; set; }

        public bool CalibrationEnabled { get; set; }

        public string TokenStr { get; set; } = string.Empty;

        public ICommand MoveRightCommand { get; set; }

        public ICommand MoveLeftCommand { get; set; }

        public ICommand GenerateCommand { get; set; }
        
        public MainWindowViewModel()
        {
            securityService = new SecurityService();

            LeftDevices = JsonSerializer.Deserialize<ObservableCollection<string>>(File.ReadAllText(deviceList)) ?? new();
            MoveRightCommand = ReactiveCommand.Create(MoveRight);
            MoveLeftCommand = ReactiveCommand.Create(MoveLeft);
            GenerateCommand = ReactiveCommand.Create(Generate);
        }

        private void MoveRight()
        {
            if (string.IsNullOrEmpty(SelectedLeftDevice))
            {
                return;
            }

            RightDevices.Add(SelectedLeftDevice);
            _ = LeftDevices.Remove(SelectedLeftDevice);
        }

        private void MoveLeft()
        {
            if (string.IsNullOrEmpty(SelectedRightDevice))
            {
                return;
            }

            LeftDevices.Add(SelectedRightDevice);
            _ = RightDevices.Remove(SelectedRightDevice);
        }

        private void Generate()
        {
            TokenInfo tokenInfo = new()
            {
                DevToolEnabled = DevToolEnabled,
                CalibrationEnabled = CalibrationEnabled,
                Expires = Expires.DateTime,
                Devices = RightDevices.ToList()
            };

            securityService.SignData(JsonSerializer.Serialize(tokenInfo), out string data, out string signature);
            TokenStr = $"{data}.{signature}";
            this.RaisePropertyChanged(nameof(TokenStr));
        }
    }

}
