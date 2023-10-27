using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Business.Common.Response;
using ToDoApp.Business.SqlServer.Bo.User;
using ToDoApp.Business.SqlServer.Business.Interface;
using ToDoApp.Business.SqlServer.Models;

namespace ToDoApp.Business.SqlServer.Business
{

    public class UserBusiness:IUserBusiness
    {
        readonly ToDoAppDbContext dbContext;

        public UserBusiness()
        {
            dbContext = new ToDoAppDbContext();
        }

        public ResponseDto Save(UserSaveCriteriaBo userSaveCriteriaBo)
        {
            int id = userSaveCriteriaBo.Id;
            string userName = userSaveCriteriaBo.Username;
            string password = userSaveCriteriaBo.Password;
            if (id == 0)
            {
                User userBo = dbContext.Users.FirstOrDefault(x => x.Username == userName);

                if (userBo == null)
                {
                    User user = new User()
                    {
                        Id = id,
                        Username = userName,
                        Password = password,
                        CreateDate = DateTime.Now,
                        CreateUserId = id
                    };
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                    return new ResponseDto().Success(id);
                }
                else
                {
                    return new ResponseDto().Failed("This Username Already using");
                }
            }
            else
            {

                User user = dbContext.Users.Find(id);

                if (user == null)
                {
                    return new ResponseDto().Failed("User Not Found!");
                }

                user.Username = userName;
                user.Password = password;
                dbContext.SaveChanges();
                return new ResponseDto().Success(id);
            }

        }

        public int GetId(string userName)
        {
            User user = dbContext.Users.FirstOrDefault(p => p.Username == userName);
            return user.Id;

        }
        public void Dispose()
        {
            if (dbContext == null) return;

            dbContext.Dispose();
        }
    }
}
