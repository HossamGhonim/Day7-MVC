namespace MVC2.Models
{
    public class DepartmentVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }

        public int StdCount { get; set; }
        public int InsCount { get; set; }

        public List<string> InsNames { get; set; } = new();
        public List<string> StdNames { get; set; } = new();
    }
}
