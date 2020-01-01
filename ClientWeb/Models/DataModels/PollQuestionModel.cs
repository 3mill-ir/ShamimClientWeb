using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class PollQuestionModel
    {
        public PollQuestionModel()
        {
            PollAnswer = new List<PollAnswerDataModel>();
        }
        public int ID { get; set; }
        public string Question { get; set; }
        public Nullable<DateTime> CreatedOnUTC { get; set; }
        public string StartDateOnUTCHelper { get; set; }
        public string EndDateOnUTCHelper { get; set; }
        public Nullable<DateTime> StartDateOnUTC { get; set; }
        public Nullable<DateTime> EndDateOnUTC { get; set; }
        public List<PollAnswerDataModel> PollAnswer { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public bool Active { get; set; }
        public string ChartType { get; set; }
        public string F_UserID { get; set; }
    }

    public class PollAnswerDataModel
    {
        public PollAnswerDataModel()
        {
            PollLog = new List<PollLogDataModel>();
        }
        public string IPAddress { get; set; }
        public string Device { get; set; }

        public virtual List<PollLogDataModel> PollLog { get; set; }

        public int ID { get; set; }

        public string Text { get; set; }

        public int AnswerKey { get; set; }

        public int Score { get; set; }

        public Nullable<int> F_PollQuestionID { get; set; }

        public string Color { get; set; }
    }

    public class PollLogDataModel
    {
        public int ID { get; set; }
        public string IPAddress { get; set; }
        public string F_UserID { get; set; }
        public string Device { get; set; }
        public Nullable<System.DateTime> CreatedOnUTC { get; set; }
        public Nullable<int> F_PollAnswerID { get; set; }
    }
}