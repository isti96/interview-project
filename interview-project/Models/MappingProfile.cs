using AutoMapper;
using interview_project.Database.Entities;

namespace interview_project.Models;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CompanyModel, Company>();
        CreateMap<Company, CompanyModel>();
    }
}
