using Microsoft.Extensions.Configuration;
using PaperTrail.Module.Login.Models;
using PaperTrail.Module.Login.Services;
using PaperTrail.Storage.Entitys;
using PaperTrail.Storage.Enums;
using PaperTrail.Utilities;
using Si.CoreHub.OperateResult;
using Si.EntityFramework.Extension.Abstraction;
using Si.EntityFramework.PermGuard.Handlers;

namespace PaperTrail.Module.Login.ServicesImpl
{
    public class LoginServiceImpl : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration configuration;
        private readonly TokenManager _jwtManager;
        public LoginServiceImpl(IUnitOfWork unitOfWork, IConfiguration packConfiguration = null, TokenManager jwtManager = null)
        {
            _unitOfWork = unitOfWork;
            configuration = packConfiguration;
            _jwtManager = jwtManager;
        }
        public async Task<Result> Login(LoginRequest request)
        {
            var key = configuration.GetValue<string>("AesConfig:Key");
            var iv = configuration.GetValue<string>("AesConfig:IV");
            var encryptePwd = StableAesCrypto.Encrypt(request.Password, key, iv);
            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(x => x.Account == request.Account && x.PasswordHash == encryptePwd);
            if (user == null)
            {
                return Result.Failed("账号或密码错误");
            }
            var token = _jwtManager.GenerateToken(user.Id, user.Name, new List<string>
            {
                RoleConst.Admin
            });
            return Result.Successed(token, "登录成功");
        }
    }
}
