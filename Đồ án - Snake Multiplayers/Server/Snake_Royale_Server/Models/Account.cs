using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Royale_Server.Models
{
    public class Account
    {
        [Key]
        public string Username { get; set; }

        public int GamePlayed { get; set; } = 0;

        public long MaxScore { get; set; } = 0;

    }
}
