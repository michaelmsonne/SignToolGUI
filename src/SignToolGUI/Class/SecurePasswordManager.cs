using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static SignToolGUI.Class.FileLogger;

namespace SignToolGUI.Class
{
    /// <summary>
    /// Provides secure password encryption/decryption using machine-specific keys
    /// </summary>
    public static class SecurePasswordManager
    {
        private const int KeySize = 256; // AES-256
        private const int IvSize = 128;  // AES block size
        private const int SaltSize = 256; // Salt size in bits
        private const int DerivationIterations = 100000; // PBKDF2 iterations (increased for better security)

        /// <summary>
        /// Generates a machine-specific encryption key based on hardware identifiers
        /// </summary>
        /// <returns>Base64 encoded machine-specific key</returns>
        private static string GenerateMachineSpecificKey()
        {
            try
            {
                var machineInfo = new StringBuilder();

                // Combine multiple hardware/system identifiers for uniqueness
                machineInfo.Append(Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "UNKNOWN_PROCESSOR");
                machineInfo.Append(Environment.MachineName);
                machineInfo.Append(Environment.OSVersion.Platform.ToString());
                machineInfo.Append(Environment.OSVersion.Version.ToString());
                machineInfo.Append(Environment.UserDomainName);
                machineInfo.Append(Environment.Is64BitOperatingSystem ? "x64" : "x86");

                // Add additional entropy from system drive serial (if available)
                try
                {
                    var systemDrive = Environment.SystemDirectory.Substring(0, 3);
                    var driveInfo = new DriveInfo(systemDrive);
                    if (driveInfo.IsReady)
                    {
                        // Note: VolumeLabel might not always be available, but we try
                        machineInfo.Append(driveInfo.DriveFormat);
                        machineInfo.Append(driveInfo.TotalSize.ToString());
                    }
                }
                catch
                {
                    // Ignore drive info errors, we have other entropy sources
                    machineInfo.Append("DEFAULT_DRIVE_ENTROPY");
                }

                // Create a hash of the combined information
                using (var sha256 = SHA256.Create())
                {
                    var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(machineInfo.ToString()));
                    return Convert.ToBase64String(hash);
                }
            }
            catch (Exception ex)
            {
                Message($"Error generating machine-specific key: {ex.Message}", EventType.Error, 3031);
                // Fallback to a machine name based key if hardware info is unavailable
                using (var sha256 = SHA256.Create())
                {
                    var fallbackData = $"FALLBACK_{Environment.MachineName}_{Environment.UserName}";
                    var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(fallbackData));
                    return Convert.ToBase64String(hash);
                }
            }
        }

        /// <summary>
        /// Encrypts a password using AES with machine-specific key derivation
        /// </summary>
        /// <param name="plainText">Password to encrypt</param>
        /// <returns>Base64 encoded encrypted password with metadata</returns>
        public static string EncryptPassword(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            try
            {
                var machineKey = GenerateMachineSpecificKey();
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                // Generate random salt and IV
                var salt = GenerateRandomBytes(SaltSize / 8);
                var iv = GenerateRandomBytes(IvSize / 8);

                // Derive key from machine-specific data and salt
                using (var pbkdf2 = new Rfc2898DeriveBytes(machineKey, salt, DerivationIterations))
                {
                    var keyBytes = pbkdf2.GetBytes(KeySize / 8);

                    using (var aes = Aes.Create())
                    {
                        aes.KeySize = KeySize;
                        aes.BlockSize = IvSize;
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;
                        aes.Key = keyBytes;
                        aes.IV = iv;

                        using (var encryptor = aes.CreateEncryptor())
                        using (var msEncrypt = new MemoryStream())
                        {
                            // Write salt and IV first
                            msEncrypt.Write(salt, 0, salt.Length);
                            msEncrypt.Write(iv, 0, iv.Length);

                            // Encrypt the password
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                                csEncrypt.FlushFinalBlock();
                            }

                            return Convert.ToBase64String(msEncrypt.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message($"Error encrypting password: {ex.Message}", EventType.Error, 3032);
                throw new InvalidOperationException("Failed to encrypt password", ex);
            }
        }

        /// <summary>
        /// Decrypts a password using AES with machine-specific key derivation
        /// </summary>
        /// <param name="cipherText">Base64 encoded encrypted password</param>
        /// <returns>Decrypted password</returns>
        public static string DecryptPassword(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            try
            {
                var machineKey = GenerateMachineSpecificKey();
                var cipherBytes = Convert.FromBase64String(cipherText);

                // Extract salt, IV, and encrypted data
                var saltSize = SaltSize / 8;
                var ivSize = IvSize / 8;

                if (cipherBytes.Length < saltSize + ivSize)
                {
                    throw new ArgumentException("Invalid cipher text format");
                }

                var salt = new byte[saltSize];
                var iv = new byte[ivSize];
                var encryptedData = new byte[cipherBytes.Length - saltSize - ivSize];

                Array.Copy(cipherBytes, 0, salt, 0, saltSize);
                Array.Copy(cipherBytes, saltSize, iv, 0, ivSize);
                Array.Copy(cipherBytes, saltSize + ivSize, encryptedData, 0, encryptedData.Length);

                // Derive key from machine-specific data and extracted salt
                using (var pbkdf2 = new Rfc2898DeriveBytes(machineKey, salt, DerivationIterations))
                {
                    var keyBytes = pbkdf2.GetBytes(KeySize / 8);

                    using (var aes = Aes.Create())
                    {
                        aes.KeySize = KeySize;
                        aes.BlockSize = IvSize;
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;
                        aes.Key = keyBytes;
                        aes.IV = iv;

                        using (var decryptor = aes.CreateDecryptor())
                        using (var msDecrypt = new MemoryStream(encryptedData))
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            var decryptedBytes = new byte[encryptedData.Length];
                            var bytesRead = csDecrypt.Read(decryptedBytes, 0, decryptedBytes.Length);
                            return Encoding.UTF8.GetString(decryptedBytes, 0, bytesRead);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message($"Error decrypting password: {ex.Message}", EventType.Error, 3033);
                // Return empty string if decryption fails (might be old format or corrupted)
                return string.Empty;
            }
        }

        /// <summary>
        /// Migrates passwords encrypted with the old StringCipher method to the new secure method
        /// </summary>
        /// <param name="oldEncryptedPassword">Password encrypted with StringCipher</param>
        /// <param name="oldPassPhrase">The passphrase used with StringCipher</param>
        /// <returns>Password encrypted with new method, or empty string if migration fails</returns>
        public static string MigrateFromStringCipher(string oldEncryptedPassword, string oldPassPhrase)
        {
            try
            {
                // Try to decrypt with old StringCipher method
                var decryptedPassword = StringCipher.Decrypt(oldEncryptedPassword, oldPassPhrase);

                // Re-encrypt with new secure method
                var newEncryptedPassword = EncryptPassword(decryptedPassword);

                Message("Successfully migrated old encrypted password to new encryption method", EventType.Information, 3034);
                return newEncryptedPassword;
            }
            catch (Exception ex)
            {
                Message($"Failed to migrate old encrypted password: {ex.Message}", EventType.Warning, 3035);
                return string.Empty;
            }
        }

        /// <summary>
        /// Generates cryptographically secure random bytes
        /// </summary>
        /// <param name="size">Number of bytes to generate</param>
        /// <returns>Array of random bytes</returns>
        private static byte[] GenerateRandomBytes(int size)
        {
            var bytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }

        /// <summary>
        /// Validates if the current machine can decrypt a given encrypted password
        /// </summary>
        /// <param name="encryptedPassword">Encrypted password to test</param>
        /// <returns>True if the password can be decrypted on this machine</returns>
        public static bool CanDecryptOnThisMachine(string encryptedPassword)
        {
            try
            {
                var decrypted = DecryptPassword(encryptedPassword);
                return !string.IsNullOrEmpty(decrypted);
            }
            catch
            {
                return false;
            }
        }
    }
}