
using genvcaredashboardsAPI.Models.General;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Genworks.API.Helpers
{
    public static class SecurityHelper
    {
        public static string DoctorUserType = "doctor";
        public static string PatientUserType = "patient";
        public static string[] EncryptUserTypes = { "patient" };
        //https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-6.0
        public static SecurityDto GeneratePassword(string password = "", string salt = "")
        {
            if (string.IsNullOrEmpty(password)) password = Random(8);

            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] slt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(slt);
            }

            if (!string.IsNullOrEmpty(salt))
            {
                slt = Convert.FromBase64String(salt);
            }

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: slt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));


            return new SecurityDto
            {
                Password = password,
                Salt = Convert.ToBase64String(slt),
                PasswordHash = hashed
            };

        }

        private static string Random(int length)
        {
            string allowed = "0123456789";
            return new string(allowed
                .OrderBy(o => Guid.NewGuid())
                .Take(length)
                .ToArray());
        }

        //public static void GenerateToken(PersonMaster person, PersonRole role, int personmappingID, Models.Authenticate.LoginDto lgin, int refreshtokenexpiry, string jwtKey)
        //{
        //    string mobile = EncryptUserTypes.Contains(role.Name.ToLower()) ? person.MobileNumber.DecryptText() : person.MobileNumber;
        //    string email = EncryptUserTypes.Contains(role.Name.ToLower()) ? person.EmailId.DecryptText() : person.EmailId;

        //    var key = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(jwtKey));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var claims = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]{
        //             new Claim("personmappingid", personmappingID.ToString()),
        //                             new Claim("personid", person.Id.ToString())
        //                            , new Claim("mobile",mobile)
        //                            , new Claim("email",email)
        //                            , new Claim("usertype", role.Name)
        //            }),
        //        Expires = DateTime.Now.AddMinutes(refreshtokenexpiry),
        //        SigningCredentials = creds

        //    };

        //    var tokenhandler = new JwtSecurityTokenHandler();
        //    var token = tokenhandler.CreateToken(claims);
        //    lgin.Token = new JwtSecurityTokenHandler().WriteToken(token);
        //    lgin.RefreshToken = Guid.NewGuid().ToString();
        //}

        static string textEncryptionKey = "AF2E89547EAC457893CFAC31418F847C";
        static bool handleError = true;
        public static string EncryptText(this string plainInput)
        {
            if (string.IsNullOrEmpty(plainInput)) return plainInput;
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(textEncryptionKey);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainInput);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptWithError(string cipherText)
        {
            handleError = false;
            return DecryptText(cipherText);
        }

        public static string DecryptText(this string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;
            string decryptedText = string.Empty;
            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(textEncryptionKey);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                decryptedText = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (handleError)
                {
                    decryptedText = cipherText;
                }
                else
                {
                    throw ex;
                }
            }

            return decryptedText;
        }

        public static string IgnoreDelimeters(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace(",", "");
            }
            return text;
        }

        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            if (text.IsNormalized(NormalizationForm.FormD))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

    }
}
