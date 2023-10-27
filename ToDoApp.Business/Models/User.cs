using System;
using System.Collections.Generic;

namespace ToDoApp.Business.SqlServer.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? ToDoListId { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateUserId { get; set; }

    public virtual ToDo? ToDoList { get; set; }

    public virtual ICollection<ToDo> ToDos { get; } = new List<ToDo>();
}
