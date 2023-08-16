using CommonLayer.Model;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interfaces
{
    public interface ICollabRepo
    {
        public CollabEntity CreateCollab(CollabCreateModel model, long UserID, long NoteID);
        public List<CollabEntity> GetAllCollabs();
        public void DeleteACollab(long CollabID);
    }
}
