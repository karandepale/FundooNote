using CommonLayer.Model;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoLayer.Services
{
    public class CollabRepo : ICollabRepo 
    {
        private readonly FundooContext fundooContext;
        private readonly IScopedUserIdService scopedUserIdService;

        public CollabRepo(FundooContext fundooContext , IScopedUserIdService scopedUserIdService)
        {
            this.fundooContext = fundooContext;
            this.scopedUserIdService = scopedUserIdService;
        }

        // CREATE COLLAB LOGIC IMPLEMENTATION:-
        public CollabEntity CreateCollab(CollabCreateModel model , long NoteID)
        {
            try
            {
                var userId = scopedUserIdService.UserId;


                CollabEntity collabEntity = new CollabEntity();
                collabEntity.Email = model.Email;
                collabEntity.UserID = userId;
                collabEntity.NoteID = NoteID;

                fundooContext.Collab.Add(collabEntity);
                fundooContext.SaveChanges();

                if(collabEntity != null)
                {
                    return collabEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw(ex);
            }
        }


        // GET LIST OF COLLABS LOGIC IMPLEMENTATION :-
        public List<CollabEntity> GetAllCollabs(long NoteID)
        {
            try
            {
                var result = fundooContext.Collab.Where(data => data.NoteID == NoteID).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        // DELETE A COLLAB :-
        public void DeleteACollab(long CollabID)
        {
            try
            {
                var collabEntity = fundooContext.Collab.FirstOrDefault(c => c.CollabID == CollabID);

                if (collabEntity != null)
                {
                    fundooContext.Collab.Remove(collabEntity);
                    fundooContext.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Collaboration not found for the given CollabID.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






    }
}
