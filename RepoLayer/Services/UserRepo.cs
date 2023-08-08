using CommonLayer.Model;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Services
{
    public class UserRepo : IUserRepo
    {
		private readonly FundooContext fundooContext;
        public UserRepo(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        // USER REGISTRATION METHOD IMPLEMENTATION:-
		public UserEntity UserRegistration(UserRegistrationModel model)
        {
			try
			{
				UserEntity userEntity = new UserEntity();
				userEntity.FirstName = model.FirstName;
				userEntity.LastName = model.LastName;
				userEntity.DateOfBirth = model.DateOfBirth;
				userEntity.Email = model.Email;
				userEntity.Password = model.Password;

				fundooContext.Users.Add(userEntity);
				fundooContext.SaveChanges();

				if(userEntity != null)
				{
					return userEntity;
				}
				else
				{
					return null;
				}

			}
			catch (Exception ex )
			{
				throw(ex);
			}
        }







    }
}
