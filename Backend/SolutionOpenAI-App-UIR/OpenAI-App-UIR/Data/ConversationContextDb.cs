using Microsoft.EntityFrameworkCore;
using OpenAI_App_UIR.Models;

namespace OpenAI_App_UIR.Data
{
    public class ConversationContextDb : DbContext
    {
        public ConversationContextDb(DbContextOptions<ConversationContextDb> options) : base(options)
        {
        }

        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Response> Responses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Conversation>().HasData(
                new Conversation { Id = 1 },
                new Conversation { Id = 2 }
            );

            modelBuilder.Entity<Question>().HasData(
                new Question { Id = 1, Text = "Question 1", ConversationId = 1 },
                new Question { Id = 2, Text = "Question 2", ConversationId = 1 },
                new Question { Id = 3, Text = "Question 3", ConversationId = 2 }
            );
        }
    }
}
