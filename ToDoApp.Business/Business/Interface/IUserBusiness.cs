using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Business.Common.Response;
using ToDoApp.Business.SqlServer.Bo.User;

namespace ToDoApp.Business.SqlServer.Business.Interface
{
    public interface IUserBusiness: IDisposable
    {

        public ResponseDto Save(UserSaveCriteriaBo userSaveCriteriaBo);
        public ResponseDto GetId(string userName);
    }
}
