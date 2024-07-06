namespace OpenAI_App_UIR.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
