using BusinessLayer.Interfaces;
using CommonLayer.Model;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBusiness : ILabelBusiness
    {
        private readonly ILabelRepo labelRepo;
        public LabelBusiness(ILabelRepo labelRepo)
        {
            this.labelRepo = labelRepo;
        }

        public LabelsEntity CreateLabel(LabelCreateModel model, long NoteID)
        {
            try
            {
                return labelRepo.CreateLabel(model, NoteID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public List<LabelsEntity> GetAllLabels(long NoteId)
        {
            try
            {
                return labelRepo.GetAllLabels(NoteId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public LabelsEntity UpdateLabel(LabelUpdateModel model, long LabelID)
        {
            try
            {
                return labelRepo.UpdateLabel(model, LabelID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        public void DeleteLabel(long LabelID)
        {
            try
            {
                 labelRepo.DeleteLabel(LabelID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }




    }
}
