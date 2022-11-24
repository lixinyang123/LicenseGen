﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace KeyGen.Services
{
    public class SecurityService
    {
        private readonly string tokenPath = "Assets/token";
        private readonly RSA rsa;

        public SecurityService()
        {
            rsa = RSA.Create();

            if (!File.Exists(tokenPath))
            {
                File.WriteAllText(tokenPath, rsa.ToXmlString(true));
            }

            rsa.FromXmlString(File.ReadAllText(tokenPath));
        }

        // 公钥加密
        public string Encrypt(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            byte[] resultBuffer = rsa.Encrypt(buffer, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(resultBuffer);
        }

        // 私钥解密
        public string Decrypt(string text)
        {
            byte[] buffer = Convert.FromBase64String(text);
            byte[] resultBuffer = rsa.Decrypt(buffer, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(resultBuffer);
        }

        // 私钥签名
        public void SignData(string text, out string data, out string signature)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            byte[] resultBuffer = rsa.SignData(buffer, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            data = Convert.ToBase64String(resultBuffer);
            signature = Convert.ToBase64String(buffer);
        }

        // 公钥验证签名
        public bool VerifyData(string data, string signature)
        {
            byte[] dataBuffer = Convert.FromBase64String(data);
            byte[] signatureBuffer = Convert.FromBase64String(signature);
            return rsa.VerifyData(signatureBuffer, dataBuffer, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}