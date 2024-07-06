namespace OpenAI_App_UIR.Models
{
    public class Response
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string Text { get; set; }
    }
}
