
namespace Snake_Royale_Server.Models
{
    public class AccountDTO
    {
        public string Username { get; set; }

        public int GamePlayed { get; set; } = 0;

        public long MaxScore { get; set; } = 0;

    }
}
