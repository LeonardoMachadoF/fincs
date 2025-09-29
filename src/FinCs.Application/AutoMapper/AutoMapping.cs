using AutoMapper;
using FinCs.Communication.Requests;
using FinCs.Communication.Responses;
using FinCs.Domain.Entities;
using Tag = FinCs.Communication.Enums.Tag;

namespace FinCs.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestRegisterExpenseJson, Expense>();
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(
                x => x.Password,
                opt
                    => opt.Ignore()
            );
        CreateMap<RequestExpenseJson, Expense>()
            .ForMember(
                dest => dest.Tags,
                config
                    => config.MapFrom(source
                        => source.Tags.Distinct())
            );


        CreateMap<Tag, Domain.Entities.Tag>()
            .ForMember(dest
                    => dest.TagName,
                config =>
                    config.MapFrom(source => source)
            );
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseRegisterExpenseJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<Expense, ResponseExpenseJson>();
        CreateMap<User, ResponseUserProfileJson>();
    }
}