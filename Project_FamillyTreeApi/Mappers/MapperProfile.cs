using AutoMapper;
using BusinessObject.DataAccess;

namespace Project_FamillyTreeApi.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Album, AlbumAPI>().ReverseMap();
            CreateMap<StudyPromotion, StudyPromotionAPI>().ReverseMap();
            CreateMap<Relationship, RelationshipAPI>().ReverseMap();
            CreateMap<Relative, RelativeAPI>().ReverseMap();
            CreateMap<FamilyMember, FamilyMemberAPI>().ReverseMap();
        }
    }
}
