namespace Project_FamillyTreeApi.Mappers
{
    public class AccountAPI
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public int MemberId { get; set; }
    }
}
