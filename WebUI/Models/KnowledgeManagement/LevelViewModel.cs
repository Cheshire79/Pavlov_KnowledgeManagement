namespace WebUI.Models.KnowledgeManagement
{
    public class LevelViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Order { get; set; }
        public override string ToString()
        {
            return Id.ToString();
        }
    }
}