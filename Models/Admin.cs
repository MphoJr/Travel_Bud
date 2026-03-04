namespace Travel_Bud.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // In production, store hashed passwords!
    }
}
