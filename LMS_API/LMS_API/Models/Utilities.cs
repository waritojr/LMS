using LMS_API.Entities;
using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LMS_API.Models
{
    public class Utilities : IUtilities
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostingEnvironment;

        public Utilities(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }


        public string SetUpHTML(UserEnt data, string temp_key)
        {
            string file_path = Path.Combine(_hostingEnvironment.ContentRootPath, "MailTemplates\\Password.html");
            string file_html = File.ReadAllText(file_path);
            file_html = file_html.Replace("@@Nombre", data.full_name);
            file_html = file_html.Replace("@@ClaveTemporal", temp_key);
            file_html = file_html.Replace("@@Link", "https://localhost:7148/Login/ChangeTempKey?q=" + Encrypt(data.id_user.ToString()));

            return file_html;
        }

        public string GenerateCode()
        {
            int length = 4;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public void SendEmail(string Addressee, string Subject, string Message)
        {
            string mail_smtp = _configuration.GetSection("Keys:mail_smtp").Value;
            string key_smtp = _configuration.GetSection("Keys:key_smtp").Value;

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(Addressee));
            msg.From = new MailAddress(mail_smtp);
            msg.Subject = Subject;
            msg.Body = Message;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(mail_smtp, key_smtp);
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Send(msg);
        }

        public string Encrypt(string texto)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("EzfjS0IHnNSjv0jo");
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(texto);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string Decrypt(string texto)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(texto);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("EzfjS0IHnNSjv0jo");
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public string GenerateToken(string id_user, string id_rol)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("username", Encrypt(id_user)));
            claims.Add(new Claim("userrol", Encrypt(id_rol)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Ty1UELmVFKQmMD4af0a4jvfZS30cXu3U"));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
