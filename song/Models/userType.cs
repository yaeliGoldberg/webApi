namespace user.Models
{
    public class userType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int age { get; set; }
        public string Role { get; set; } = "user"; // ברירת מחדל – user
    }
} 