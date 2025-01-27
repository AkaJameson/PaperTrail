namespace PaperTrail.Api.Models
{
    public class CurrentUser : ICurrentUser
    {
        public long? UserId { get; set; }
    }
}
