namespace Test1.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            return this.Id == (obj as Project)?.Id;
        }

    }
}