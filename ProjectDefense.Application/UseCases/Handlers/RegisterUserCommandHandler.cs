using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Domain.Entities;
using System.Text;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailSender;

        public RegisterUserCommandHandler(UserManager<User> userManager, IEmailService emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = $"https://localhost:7059/Identity/Account/ConfirmEmail?userId={user.Id}&code={code}";

            await _emailSender.SendEmailAsync(request.Email, "Confirm your email",
                $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

            return result;
        }
    }
}