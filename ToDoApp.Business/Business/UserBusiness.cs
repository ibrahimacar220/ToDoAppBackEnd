using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match matchGmail = regexMail.Match(userName);
            if (!matchGmail.Success)
            {
                return new ResponseDto().Failed("Plase Enter a Valid Mail like this: adc@exemple.com");
            }

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

        public ResponseDto GetId(string userName)
        {
            try
            {
                User user = dbContext.Users.FirstOrDefault(p => p.Username == userName);
                if (user == null)
                {
                    return new ResponseDto().Failed("UserName or Password Wrong");
                }
                return new ResponseDto().Success(user.Id);
            }
            catch (Exception ex)
            {

                return new ResponseDto().FailedWithException(ex);
            }
          

        }
        public void Dispose()
        {
            if (dbContext == null) return;

            dbContext.Dispose();
        }
    }
}
