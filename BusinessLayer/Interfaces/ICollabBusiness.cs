using CommonLayer.Model;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ICollabBusiness
    {
        public CollabEntity CreateCollab(CollabCreateModel model, long UserID, long NoteID);
        public List<CollabEntity> GetAllCollabs();
        public void DeleteACollab(long CollabID);
    }
}
