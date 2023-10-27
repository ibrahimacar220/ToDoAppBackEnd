using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Business.Common.Response;
using ToDoApp.Business.SqlServer.Bo.ToDo;
using ToDoApp.Business.SqlServer.Business.Interface;
using ToDoApp.Business.SqlServer.Models;

namespace ToDoApp.Business.SqlServer.Business
{
    public class ToDoBusiness:ITodoBusiness
    {
        readonly ToDoAppDbContext dbContext;

        public ToDoBusiness()
        {
            dbContext = new ToDoAppDbContext();
        }

        public ResponseDto Save(ToDoSaveCriteriaBo toDoSaveCriteriaBo)
        {
            int id = toDoSaveCriteriaBo.Id;
            int userId = toDoSaveCriteriaBo.UserId;
            string tasks = toDoSaveCriteriaBo.Tasks;
            bool isTaskCompleted = toDoSaveCriteriaBo.IsTaskCompleted;
            if (id == 0)
            {
                ToDo toDo = new ToDo()
                {
                    Id = id,
                    UserId = userId,
                    Tasks = tasks,
                    IsTaskCompleted = false,
                    CreateDate = DateTime.Now,
                    CreateUserId = userId
                };
                dbContext.ToDos.Add(toDo);
                dbContext.SaveChanges();
                return new ResponseDto().Success(id);
            }
            else
            {
                ToDo toDo = dbContext.ToDos.Find(id);

                if (toDo == null)
                {
                    return new ResponseDto().Failed("ToDo Not Found!");
                }

                toDo.Tasks = tasks;
                toDo.IsTaskCompleted = isTaskCompleted;
                dbContext.SaveChanges();
                return new ResponseDto().Success(id);
            }
        }

        public List<ToDo> GetList(bool isActive) 
        {
            List<ToDo> toDoList = dbContext.ToDos.Where(p => p.IsTaskCompleted == isActive).ToList();
            return toDoList;
        }

        public List<ToDo> GetById(int id)
        {
            List<ToDo> toDoList = dbContext.ToDos.Where(p => p.UserId == id).ToList();
            return toDoList;
        }

        public ResponseDto Delete(int id)
        {
            ToDo toDo = dbContext.ToDos.Find(id);

            if (toDo == null)
            {
                return new ResponseDto().Failed("User Not Found");
            }

            dbContext.ToDos.Remove(toDo);
            dbContext.SaveChanges();
            return new ResponseDto().Success(id);
        }
        public ResponseDto ChangeTaskCompleted(int id)
        {
            ToDo toDo = dbContext.ToDos.Find(id);
            toDo.IsTaskCompleted = !toDo.IsTaskCompleted;

            dbContext.SaveChanges();

            return new ResponseDto().Success(id);
        }

        public void Dispose()
        {
            if (dbContext == null) return;

            dbContext.Dispose();
        }
    }
}
