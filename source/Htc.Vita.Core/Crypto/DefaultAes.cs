﻿using System;
using System.IO;
using System.Security.Cryptography;
using Htc.Vita.Core.Log;

namespace Htc.Vita.Core.Crypto
{
    public class DefaultAes : Aes
    {
        private const int KeySize128BitInBit = 128;
        private const int KeySize128BitInByte = KeySize128BitInBit / 8;
        private const int KeySize192BitInBit = 192;
        private const int KeySize192BitInByte = KeySize192BitInBit / 8;
        private const int KeySize256BitInBit = 256;
        private const int KeySize256BitInByte = KeySize256BitInBit / 8;
        private const int IvSize128BitInBit = 128;
        private const int IvSize128BitInByte = IvSize128BitInBit / 8;
        private const int SaltSize128BitInBit = 128;
        private const int SaltSize128BitInByte = SaltSize128BitInBit / 8;

        private static System.Security.Cryptography.CipherMode ConvertToImpl(CipherMode cipherMode)
        {
            if (cipherMode == CipherMode.Cbc)
            {
                return System.Security.Cryptography.CipherMode.CBC;
            }
            Logger.GetInstance().Error("unknown cipher mode: " + cipherMode);
            return System.Security.Cryptography.CipherMode.CBC;
        }

        private static System.Security.Cryptography.PaddingMode ConvertToImpl(PaddingMode paddingMode)
        {
            if (paddingMode == PaddingMode.Pkcs7)
            {
                return System.Security.Cryptography.PaddingMode.PKCS7;
            }
            Logger.GetInstance().Error("unknown padding mode: " + paddingMode);
            return System.Security.Cryptography.PaddingMode.PKCS7;
        }

        protected override byte[] OnDecrypt(byte[] input, string password)
        {
            var encryptedDataLength = input.Length - SaltSize128BitInByte;
            if (encryptedDataLength <= 0)
            {
                Logger.GetInstance().Error("input cipher text is malformed");
                return null;
            }

            var salt = new byte[SaltSize128BitInByte];
            var encryptedData = new byte[encryptedDataLength];
            using (var resultStream = new MemoryStream(input))
            {
                using (var binaryReader = new BinaryReader(resultStream))
                {
                    binaryReader.Read(
                            salt,
                            0,
                            SaltSize128BitInByte
                    );
                    binaryReader.Read(
                            encryptedData,
                            0,
                            encryptedDataLength
                    );
                }
            }

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt))
            {
                var key = deriveBytes.GetBytes(KeySize256BitInByte);
                var iv = deriveBytes.GetBytes(IvSize128BitInByte);
                return OnDecrypt(
                        encryptedData,
                        key,
                        iv
                );
            }
        }

        protected override byte[] OnDecrypt(byte[] input, byte[] key, byte[] iv)
        {
            if (iv == null || iv.Length != IvSize128BitInByte)
            {
                throw new ArgumentException("iv size is not match");
            }

            if (key.Length != KeySize128BitInByte
                    && key.Length != KeySize192BitInByte
                    && key.Length != KeySize256BitInByte)
            {
                throw new ArgumentException("key size is not match");
            }

            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                if (aes == null)
                {
                    Logger.GetInstance().Info("can not create aes instance");
                    return null;
                }

                aes.Mode = ConvertToImpl(Cipher);
                aes.Padding = ConvertToImpl(Padding);

                using (var decryptor = aes.CreateDecryptor(key, iv))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(
                                    input,
                                    0,
                                    input.Length
                            );
                        }
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        protected override byte[] OnEncrypt(byte[] input, string password)
        {
            var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize128BitInByte);
            var salt = deriveBytes.Salt;
            var key = deriveBytes.GetBytes(KeySize256BitInByte);
            var iv = deriveBytes.GetBytes(IvSize128BitInByte);

            var encryptedBytes = OnEncrypt(
                    input,
                    key,
                    iv
            );

            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(salt);
                    binaryWriter.Write(encryptedBytes);
                }
                return memoryStream.ToArray();
            }
        }

        protected override byte[] OnEncrypt(byte[] input, byte[] key, byte[] iv)
        {
            if (iv == null || iv.Length != IvSize128BitInByte)
            {
                throw new ArgumentException("iv size is not match");
            }

            if (key.Length != KeySize128BitInByte
                    && key.Length != KeySize192BitInByte
                    && key.Length != KeySize256BitInByte)
            {
                throw new ArgumentException("key size is not match");
            }

            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                if (aes == null)
                {
                    Logger.GetInstance().Info("can not create aes instance");
                    return null;
                }

                aes.Mode = ConvertToImpl(Cipher);
                aes.Padding = ConvertToImpl(Padding);

                using (var encryptor = aes.CreateEncryptor(key, iv))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(
                                    input,
                                    0,
                                    input.Length
                            );
                        }
                        return memoryStream.ToArray();
                    }
                }
            }
        }
    }
}