using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAI_App_UIR.Data;
using OpenAI_App_UIR.Models;

namespace OpenAI_App_UIR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : ControllerBase
    {
        private readonly ConversationContextDb _context;

        public ConversationsController(ConversationContextDb context)
        {
            _context = context;
        }

        // GET: api/Conversations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conversation>>> GetConversations()
        {
            return await _context.Conversations
                                 .Include(c => c.Questions)
                                 .ToListAsync();
        }

        // GET: api/Conversations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Conversation>> GetConversation(int id)
        {
            var conversation = await _context.Conversations
                                             .Include(c => c.Questions)
                                             .FirstOrDefaultAsync(c => c.Id == id);

            if (conversation == null)
            {
                return NotFound();
            }

            return conversation;
        }

        // POST: api/Conversations
        [HttpPost]
        public async Task<ActionResult<Conversation>> PostConversation(Conversation conversation)
        {
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConversation", new { id = conversation.Id }, conversation);
        }

        // PUT: api/Conversations/
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConversation(int id, Conversation conversation)
        {
            if (id != conversation.Id)
            {
                return BadRequest();
            }

            _context.Entry(conversation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConversationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Conversations/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConversation(int id)
        {
            var conversation = await _context.Conversations.FindAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }

            _context.Conversations.Remove(conversation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConversationExists(int id)
        {
            return _context.Conversations.Any(e => e.Id == id);
        }
    }
}
