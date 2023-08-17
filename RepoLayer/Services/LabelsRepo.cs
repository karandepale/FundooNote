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
    public class LabelsRepo : ILabelRepo
    {
        private readonly FundooContext fundooContext;
        private readonly IScopedUserIdService scopedUserIdService;

        public LabelsRepo(FundooContext fundooContext, IScopedUserIdService scopedUserIdService)
        {
            this.fundooContext = fundooContext;
            this.scopedUserIdService = scopedUserIdService;
        }



        // CREATE LABEL IMPLEMENTATION LOGIC:-
        public LabelsEntity CreateLabel(LabelCreateModel model , long NoteID)
        {
            try
            {
                var userID = scopedUserIdService.UserId;
                
                LabelsEntity label = new LabelsEntity();
                label.Title = model.Title;
                label.UserID = userID;
                label.NoteID = NoteID;

                fundooContext.Labels.Add(label);
                fundooContext.SaveChanges();

                if(label != null)
                {
                    return label;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        // GET ALL LABELS FOR A PARTICULAR NOTE:-
        public List<LabelsEntity> GetAllLabels(long NoteId)
        {
            try
            {
                var result = fundooContext.Labels.Where(data => data.NoteID == NoteId).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        //UPDATE A LABEL FOR PARTICULAR NOTE:-
        public LabelsEntity UpdateLabel(LabelUpdateModel model , long LabelID)
        {
            try
            {
                var result = fundooContext.Labels.FirstOrDefault
                    (table => table.LabelID == LabelID);

                if(result != null)
                {
                    result.Title = model.Title;
                    fundooContext.SaveChanges();
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        //DELETE A LABEL FOR PARTICULAR NOTE:-
        public void DeleteLabel(long LabelID)
        {
            try
            {
                var result = fundooContext.Labels.FirstOrDefault
                    (data => data.LabelID == LabelID);

                if(result != null)
                {
                    fundooContext.Labels.Remove(result);
                    fundooContext.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Label not found for the given LabelID.");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }






    }
}
