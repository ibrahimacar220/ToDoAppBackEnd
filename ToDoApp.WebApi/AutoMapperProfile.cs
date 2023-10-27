using AutoMapper;
using ToDoApp.Business.SqlServer.Bo.ToDo;
using ToDoApp.Business.SqlServer.Bo.User;
using ToDoApp.WebApi.Dto.ToDo;
using ToDoApp.WebApi.Dto.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ToDoApp.WebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ToDoSaveCriteriaBo, ToDoSaveCriteriaDto>().ReverseMap();
            CreateMap<UserSaveCriteriaBo,UserSaveCriteriaDto>().ReverseMap();
        }
    }
}
