using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Entity
{
    public class UserLoginResult
    {
        public UserEntity UserEntity { get; set; }
        public string JwtToken { get; set; }
    }
}
