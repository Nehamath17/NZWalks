using AutoMapper;

namespace NZWalksAPI.Profiles
{
    public class WalksProfile: Profile
    {
        public WalksProfile()
        {
            CreateMap<Models.Domains.Walk, Models.DTO.Walk>()
                .ReverseMap();

            CreateMap<Models.Domains.WalkDifficulty, Models.DTO.WalkDifficulty>()
                .ReverseMap();
        }
    }
}
