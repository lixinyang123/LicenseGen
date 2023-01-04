using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KeyGen.Services
{
    public class SecurityService
    {
        private readonly string priKeyPath = "Assets/key.pri";
        private readonly string pubKeyPath = "Assets/key.pub";

        private RSA rsa = RSA.Create();

        public SecurityService() => Generate();

        public void Generate(bool force = false)
        {
            if(!Generated || force)
            {
                rsa = RSA.Create();
                File.WriteAllText(priKeyPath, rsa.ExportRSAPrivateKeyPem());
                File.WriteAllText(pubKeyPath, rsa.ExportRSAPublicKeyPem());
            }
        }

        private bool Generated { get => File.Exists(priKeyPath) && File.Exists(pubKeyPath); }

        public string PrivateKey { get => File.ReadAllText(priKeyPath); }

        public string PublicKey { get => File.ReadAllText(pubKeyPath); }

        // 公钥加密
        public string Encrypt(string text)
        {
            rsa.ImportFromPem(File.ReadAllText(pubKeyPath));

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            byte[] resultBuffer = rsa.Encrypt(buffer, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(resultBuffer);
        }

        // 私钥解密
        public string Decrypt(string text)
        {
            rsa.ImportFromPem(File.ReadAllText(priKeyPath));

            byte[] buffer = Convert.FromBase64String(text);
            byte[] resultBuffer = rsa.Decrypt(buffer, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(resultBuffer);
        }

        // 私钥签名
        public void SignData(string text, out string data, out string signature)
        {
            rsa.ImportFromPem(File.ReadAllText(priKeyPath));

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            byte[] resultBuffer = rsa.SignData(buffer, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            data = Convert.ToBase64String(resultBuffer);
            signature = Convert.ToBase64String(buffer);
        }

        // 公钥验证签名
        public bool VerifyData(string data, string signature)
        {
            rsa.ImportFromPem(File.ReadAllText(pubKeyPath));

            byte[] dataBuffer = Convert.FromBase64String(data);
            byte[] signatureBuffer = Convert.FromBase64String(signature);
            return rsa.VerifyData(signatureBuffer, dataBuffer, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
