using API.Models;
using AutoMapper;

namespace API.GraphQL
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserForCreate, User>()
                .ForAllMembers(options =>
                    options.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}