using System;

namespace Project_FamillyTreeApi.Mappers
{
    public class StudyPromotionAPI
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public string PromotionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}
