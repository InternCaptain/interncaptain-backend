using AutoMapper;

namespace API.GraphQL
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<string, string>()
                .ForAllMembers(options =>
                    options.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}