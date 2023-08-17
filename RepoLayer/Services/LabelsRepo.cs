using CommonLayer.Model;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
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




    }
}
