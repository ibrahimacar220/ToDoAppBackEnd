using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Business.SqlServer.Bo.ToDo
{
    public class ToDoSaveCriteriaBo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Tasks { get; set; }
        public bool IsTaskCompleted { get; set; }
    }
}
