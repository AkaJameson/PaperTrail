﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperTrail.Module.Login.Models;
using PaperTrail.Module.Login.Services;
using PaperTrail.Utilities.Captcha;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Module.Login.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ICaptchaService _captcha;
        private readonly ILoginService _loginService;

        public LoginController(ICaptchaService captcha, ILoginService loginService = null)
        {
            _captcha = captcha;
            _loginService = loginService;
        }
        [AllowAnonymous]
        [HttpGet]
        public Result Captcha()
        {
            var id = Guid.NewGuid().ToString();
            var info = _captcha.GenerateCaptcha(id);
            var base64Image = Convert.ToBase64String(info.ImageBytes);
            return Result.Successed(new
            {
                id,
                imgSrc = $"data:image/png;base64,{base64Image}"
            });
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<Result> Login([FromBody] LoginRequest request, [FromQuery] string captchaId, [FromQuery] string captchaCode)
        {
            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
            {
                return Result.Failed("账号或密码不能为空");
            }
            if (_captcha.Validate(captchaId, captchaCode))
            {
                return await _loginService.Login(request);
            }
            else
            {
                return Result.Failed("验证码错误");
            }
        }
    }
}
