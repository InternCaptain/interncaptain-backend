using API.Models;

namespace API.GraphQL
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserForCreate, User>()
                .ForAllMembers(options =>
                    options.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}