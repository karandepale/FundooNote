using CommonLayer.Model;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interfaces
{
    public interface ILabelRepo
    {
        public LabelsEntity CreateLabel(LabelCreateModel model, long NoteID);
    }
}
