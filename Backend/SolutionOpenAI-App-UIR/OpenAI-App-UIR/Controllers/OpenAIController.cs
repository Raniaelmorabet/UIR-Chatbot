using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OpenAI_App_UIR.Data;
using OpenAI_App_UIR.Models;
using Azure.AI.OpenAI;

namespace OpenAI_App_UIR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {
        private readonly OpenAIClient _openAIClient;

        public OpenAIController(OpenAIClient openAIClient)
        {
            _openAIClient = openAIClient;
        }
        [HttpPost]
        public async Task<ActionResult<string>> GenerateResponse([FromBody] UserInputModel userInput, [FromServices] ConversationContextDb dbContext)
        {
            if (string.IsNullOrEmpty(userInput.Question))
            {
                return BadRequest("Question cannot be empty.");
            }

            // Generate response using Azure OpenAI
            var response = await _openAIClient.GetChatCompletionsAsync("namodaj",
                new ChatCompletionsOptions()
                {
                    Messages = {
                new ChatMessage(ChatRole.User, userInput.Question),
                    },
                    Temperature = 0.7f,
                    MaxTokens = 800,
                    NucleusSamplingFactor = 0.95f,
                    FrequencyPenalty = 0,
                    PresencePenalty = 0,
                });

            // Get the response text
            var responseText = response.Value.Choices.First().Message.Content;

            // Save conversation, question, and response to the database
            var conversation = new Conversation();
            var question = new Question { Text = userInput.Question, Conversation = conversation };
            var responseEntity = new OpenAI_App_UIR.Models.Response { Text = responseText, Question = question };


            dbContext.Conversations.Add(conversation);
            dbContext.Questions.Add(question);
            dbContext.Responses.Add(responseEntity);

            await dbContext.SaveChangesAsync();

            return Ok(responseText);
        }

    }
}
