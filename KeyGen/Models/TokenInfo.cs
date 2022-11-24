using System.Collections.Generic;

namespace KeyGen.Models
{
    public class TokenInfo
    {
        public bool DevToolEnabled { get; set; } = true;

        public bool CalibrationEnabled { get; set; } = true;

        public List<string> Devices { get; set; } = new();
    }
}
