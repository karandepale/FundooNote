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
        public UserLoginResult UserLogin(UserLoginModel model);
        public List<UserEntity> GetAllUser();
        public UserEntity GetUserByID(long UserID);
    }
}
