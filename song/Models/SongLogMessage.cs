using System;

namespace SongLog.Models
{
    public class SongLogMessage
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string SongName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
