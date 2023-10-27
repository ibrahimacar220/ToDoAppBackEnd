using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Business.Common.Response;
using ToDoApp.Business.SqlServer.Bo.ToDo;
using ToDoApp.Business.SqlServer.Business;
using ToDoApp.Business.SqlServer.Business.Interface;
using ToDoApp.Business.SqlServer.Models;
using ToDoApp.WebApi.Dto.ToDo;

namespace ToDoApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController:ControllerBase
    {
        private readonly IMapper _mapper;
        public ToDoController(IMapper mapper,ITodoBusiness todoBusiness)
        {
            _mapper = mapper;
            _todoBusiness = todoBusiness;
        }
        public ITodoBusiness _todoBusiness;

        [HttpPost("Save")]
        public ResponseDto Save(ToDoSaveCriteriaDto toDoSaveCriteriaDto)
        {
            ToDoSaveCriteriaBo toDoSaveCriteriaBo = _mapper.Map<ToDoSaveCriteriaBo>(toDoSaveCriteriaDto);
            return _todoBusiness.Save(toDoSaveCriteriaBo);
        }

        [HttpGet("TodoList")]
        public List<ToDo> GetList(bool isActive)
        {
            return _todoBusiness.GetList(isActive);
        }

        [HttpGet("GetTodoListByUserId")]
        public List<ToDo> GetById([FromQuery]int id)
        {
            return _todoBusiness.GetById(id);
        }

        [HttpDelete("Delete")]

        public ResponseDto Delete(int id) 
        {
            return _todoBusiness.Delete(id);
        }
        [HttpPost("ChangeTaskCompleted")]
        public ResponseDto ChangeTaskCompleted(int id)
        {
            return _todoBusiness.ChangeTaskCompleted(id);
        }

    }
}
