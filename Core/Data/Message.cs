using System;
namespace Core.Data
{
    public class Message
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public int Status { get; set; }
        public System.DateTime? CreateDate { get; set; }
    }
}
