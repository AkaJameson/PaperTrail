namespace PaperTrail.Module.Bloger.Models
{
    public class UpdateUserPasswordRequest
    {
        public string oldPassword { get; set; }
        public string confirmPassword { get; set; }
        public string newPassword { get; set; }
    }
}
