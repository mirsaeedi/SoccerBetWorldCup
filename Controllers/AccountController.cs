using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SoccerBet.Data;
using SoccerBet.Data.Models;
using SoccerBet.ViewModels.Command;

namespace SoccerBet.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly SoccerBetDbContext _dbContext;
        private readonly IOptions<JwtOptionConfiguration> _jwtOptionConfiguration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            SoccerBetDbContext dbContext,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<JwtOptionConfiguration> jwtOptionConfiguration)
        {
            _dbContext = dbContext;
            _jwtOptionConfiguration = jwtOptionConfiguration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Route("signin/{provider}")]
        [HttpPost]
        public IActionResult ExternalLogin(string provider)
        {
            string redirectUrl = "/signin/signin-success";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [Authorize]
        [HttpPost("account/join-group")]
        public async Task<IActionResult> JoinGroup([FromBody]JoinBetGroupCommand command)
        {
            var user = await GetCurrentUser();

            var userBetGroup = await _dbContext
                .UserBetGroups
                .SingleOrDefaultAsync(q => q.BetGroup.GroupCode == command.GroupCode
                && q.User.Id==user.Id);

            if (userBetGroup?.BetGroupId == 0)
                return BadRequest();

            var token = GenerateJwtToken(user, userBetGroup.BetGroupId);
            return Ok(new { Token = token });
        }

        [Route("signin/gettoken")]
        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            var user = await _userManager.GetUserAsync(User);

            var defaultUserBetGroup = await _dbContext.UserBetGroups
                .FirstOrDefaultAsync(q => q.UserId == user.Id);

            var token = GenerateJwtToken(user, defaultUserBetGroup?.BetGroupId);

            return Ok(new { Token= token });
        }

        [Route("signin/signin-success")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            var result = await _signInManager
                .ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return RedirectToLocal("/signin-success");
            }

            var user = new User
            {
                Email = info.Principal.FindFirst(ClaimTypes.Email)?.Value,
                UserName = info.Principal.FindFirst(ClaimTypes.Email)?.Value,
                Name = info.Principal.FindFirst(ClaimTypes.Name).Value,
                PhoneNumber = ""
            };

            if (info.LoginProvider == "Facebook")
            {
                var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                user.ImageUrl = $"https://graph.facebook.com/{identifier}/picture?type=large";
            }
            else if (info.LoginProvider == "Google")
            {
                user.ImageUrl = info.Principal.FindFirst("Image").Value;
            }
            else if (info.LoginProvider == "Twitter")
            {
                var identifier = info.Principal.FindFirstValue(ClaimTypes.Name);
                user.ImageUrl = $"https://twitter.com/{identifier}/profile_image?size=original";
            }

            var identResult = await _userManager.CreateAsync(user);

            if (identResult.Succeeded)
            {
                identResult = await _userManager.AddLoginAsync(user, info);

                if (identResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return Redirect("/signin-success");
                }
            }

            return null;
        }

        private string GenerateJwtToken(User user,long? betGroupId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim("ImageUrl", user.ImageUrl??""),
                new Claim("BetGroupId", betGroupId?.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber??"")
            };

            var issuerSigningKey = _jwtOptionConfiguration.Value.IssuerSigningKey;
            var creds = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_jwtOptionConfiguration.Value.ExpireDays);

            var token = new JwtSecurityToken(
                _jwtOptionConfiguration.Value.ValidIssuer,
                _jwtOptionConfiguration.Value.ValidAudience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            return Redirect(returnUrl);

            /*if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
//                return RedirectToAction(nameof(HomeController.Index), "Home");
            }*/
        }

        private async Task<User> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}