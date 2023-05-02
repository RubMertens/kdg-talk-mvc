using System;
using Microsoft.AspNetCore.Identity;

namespace Voting.Data.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int PossibleAnswerId { get; set; }
        public PossibleAnswer PossibleAnswer { get; set; }
        public IdentityUser User { get; set; }
        public string UserId { get; set; }

        public int? CommentId { get; set; }
    }
}
