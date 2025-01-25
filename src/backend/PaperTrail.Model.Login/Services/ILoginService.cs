using PaperTrail.Model.Login.Models;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Model.Login.Services
{
    public interface ILoginService
    {
        Task<Result> Login(LoginRequest request);
    }
}
