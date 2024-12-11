using System.Security.Cryptography;
using System.Text;

namespace Galimov_CrossPlatform_LR2_BackEnd.Models
{
    public class User
    {
        public string Login { get; set; }
        private byte[] password;
        public string Password
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var b in MD5.Create().ComputeHash(password))
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
            set { password = Encoding.UTF8.GetBytes(value); }
        }

        public bool IsAdmin => Login == "admin";

        public bool CheckPassword(string password)
        {
            return Password == new User { Password = password }.Password;
        }
    }
}