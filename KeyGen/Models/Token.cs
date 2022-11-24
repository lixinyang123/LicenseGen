namespace KeyGen.Models
{
    public class Token
    {
        // Data
        public string D { get; set; } = string.Empty;

        // Signature
        public string S { get; set; } = string.Empty;

        public Token(string data, string signature)
        {
            D = data;
            S = signature;
        }
    }
}
