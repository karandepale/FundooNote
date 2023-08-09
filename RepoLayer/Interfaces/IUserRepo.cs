using CommonLayer.Model;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interfaces
{
    public interface IUserRepo
    {
        public UserEntity UserRegistration(UserRegistrationModel model);
        public UserEntity UserLogin(UserLoginModel model);
    }
}
