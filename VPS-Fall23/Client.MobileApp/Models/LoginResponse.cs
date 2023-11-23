using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MobileApp.Models
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public UserData UserData { get; set; }
    }

}
