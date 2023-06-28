using System;

namespace Project_FamillyTreeApi.Mappers
{
    public class FamilyMemberAPI
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? FamilyId { get; set; }
    }
}
