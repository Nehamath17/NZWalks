using AutoMapper;

namespace NZWalksAPI.Profiles
{
    public class RegionsProfile: Profile
    {
        public RegionsProfile()
        {
            CreateMap<Models.Domains.Region, Models.DTO.Region>()
                .ReverseMap();

        }
    }
}
