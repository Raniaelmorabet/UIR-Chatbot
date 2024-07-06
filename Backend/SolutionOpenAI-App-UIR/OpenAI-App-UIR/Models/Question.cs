namespace OpenAI_App_UIR.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }
        public string Text { get; set; }
    }
}
