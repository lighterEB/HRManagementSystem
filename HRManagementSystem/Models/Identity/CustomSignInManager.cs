using System;
using System.Linq;
using System.Threading.Tasks;
using HRManagementSystem.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HRManagementSystem.Models.Identity
{
    public class CustomSignInManager : SignInManager<User>
    {
        private readonly UserManager<User> _userManager;

        public CustomSignInManager(
            UserManager<User> userManager, 
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IAuthenticationSchemeProvider schemes)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
            _userManager = userManager;
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                return SignInResult.Success;
            }
            else
            {
                return SignInResult.Failed;
            }
        }
    }

    public class MockHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; } = null;
    }
} 