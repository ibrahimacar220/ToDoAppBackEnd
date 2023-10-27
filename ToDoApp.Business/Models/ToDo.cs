using System;
using System.Collections.Generic;

namespace ToDoApp.Business.SqlServer.Models;

public partial class ToDo
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Tasks { get; set; } = null!;

    public bool IsTaskCompleted { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateUserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
