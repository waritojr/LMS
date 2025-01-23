using LMS_API.Entities;

namespace LMS_API.Interfaces
{
    public interface IUtilities
    {
        public string SetUpHTML(UserEnt data, string temp_key);

        public string GenerateCode();

        public void SendEmail(string Addressee, string Subject, string Message);

        public string Encrypt(string texto);

        public string Decrypt(string texto);

        public string GenerateToken(string id_user, string id_rol);
    }
}
