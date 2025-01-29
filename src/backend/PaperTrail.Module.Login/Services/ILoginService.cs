using PaperTrail.Module.Login.Models;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Module.Login.Services
{
    public interface ILoginService
    {
        Task<Result> Login(LoginRequest request);
    }
}
