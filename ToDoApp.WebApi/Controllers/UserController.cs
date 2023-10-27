using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Business.Common.Response;
using ToDoApp.Business.SqlServer.Bo.User;
using ToDoApp.Business.SqlServer.Business.Interface;
using ToDoApp.WebApi.Dto.User;

namespace ToDoApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController:ControllerBase
    {
        private readonly IMapper _mapper;
        public UserController(IMapper mapper,IUserBusiness userBusiness)
        {
            _mapper = mapper;
            _userBusiness = userBusiness;
        }
        public IUserBusiness _userBusiness;

        [HttpPost("Save")]
        public ResponseDto Save([FromBody]UserSaveCriteriaDto userSaveCriteriaDto)
        {
            UserSaveCriteriaBo userSaveCriteriaBo = _mapper.Map<UserSaveCriteriaBo>(userSaveCriteriaDto);
            return _userBusiness.Save(userSaveCriteriaBo);
        }
        [HttpGet("GetId")]
        public int GetId(string userName)
        {
            return _userBusiness.GetId(userName);
        }
    }
}
