using CommonLayer.Model;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBusiness
    {
        public LabelsEntity CreateLabel(LabelCreateModel model, long NoteID);
        public List<LabelsEntity> GetAllLabels(long NoteId);
        public LabelsEntity UpdateLabel(LabelUpdateModel model, long LabelID);
        public void DeleteLabel(long LabelID);
    }
}
