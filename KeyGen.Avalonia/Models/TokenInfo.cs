using System;
using System.Collections.Generic;

namespace AvaloniaKeyGen.Models
{
    public class TokenInfo
    {
        public DateTime Expires { get; set; } = DateTime.Now.AddDays(30);

        public bool DevToolEnabled { get; set; } = true;

        public bool CalibrationEnabled { get; set; } = true;

        public List<string> Devices { get; set; } = new();
    }
}
