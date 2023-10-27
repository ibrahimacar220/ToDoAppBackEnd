using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Business.Common.Response;
using ToDoApp.Business.SqlServer.Bo.ToDo;
using ToDoApp.Business.SqlServer.Models;

namespace ToDoApp.Business.SqlServer.Business.Interface
{
    public interface ITodoBusiness : IDisposable
    {
        public ResponseDto Save(ToDoSaveCriteriaBo toDoSaveCriteriaBo);
        public List<ToDo> GetList(bool isActive);
        public List<ToDo> GetById(int id);
        public ResponseDto Delete(int id);
        public ResponseDto ChangeTaskCompleted(int id);
    }
}
