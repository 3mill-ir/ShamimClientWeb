using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class TicketInboxModel
    {
        public TicketInboxModel()
        {
            TicketOutbox = new List<TicketOutBoxModel>();
            TicketInboxMedia = new List<TicketInboxMediaModel>();
        }
        public int ID { get; set; }
        public string TicketContent { get; set; }
        public DateTime CreatedOnUTC { get; set; }
        public string CreatedOnUTCJalali { get; set; }
        public string TicketType { get; set; }
        public string TicketFrom { get; set; }
        public int F_Ticket_Id { get; set; }


        public List<TicketOutBoxModel> TicketOutbox { get; set; }

        public List<TicketInboxMediaModel> TicketInboxMedia { get; set; }
    }
    public class TicketOutBoxModel
    {
        public int ID { get; set; }
        public string Content_One { get; set; }
        public bool isRead { get; set; }
        public string SMSText { get; set; }
        public string SMSStatusOne { get; set; }
        public string SMSStatusTwo { get; set; }
        public DateTime CreatedOnUTC { get; set; }
        public string CreatedOnUTCJalali { get; set; }
        public int F_TicketInbox_Id { get; set; }
        public string UserRole { get; set; }
    }
    public class TicketInboxMediaModel
    {
        public int ID { get; set; }
        public string MediaPath { get; set; }
        public string MediaType { get; set; }
        public int F_TicketInbox_Id { get; set; }
    }
}