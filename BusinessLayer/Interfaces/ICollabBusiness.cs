using CommonLayer.Model;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{ 
    public interface ICollabBusiness
    {
        public CollabEntity CreateCollab(CollabCreateModel model, long NoteID);
        public List<CollabEntity> GetAllCollabs(long NoteID);
        public void DeleteACollab(long CollabID);
    }
}
