using BusinessLayer.Interfaces;
using CommonLayer.Model;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepo _userRepo;
        public UserBusiness(IUserRepo _userRepo)
        {
            this._userRepo = _userRepo;
        }
        public UserEntity UserRegistration(UserRegistrationModel model)
        {
            try
            {
                return _userRepo.UserRegistration(model);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public UserLoginResult UserLogin(UserLoginModel model)
        {
            try
            {
                return _userRepo.UserLogin(model);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        public List<UserEntity> GetAllUser()
        {
            try
            {
                return _userRepo.GetAllUser();
            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }


        public UserEntity GetUserByID(long UserID)
        {
            try
            {
                return _userRepo.GetUserByID(UserID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        public string ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                return _userRepo.ForgotPassword(model);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }




    }
}
