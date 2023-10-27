namespace ToDoApp.WebApi.Dto.ToDo
{
    public class ToDoSaveCriteriaDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Tasks { get; set; }
        public bool IsTaskCompleted { get; set; }
    }
}
