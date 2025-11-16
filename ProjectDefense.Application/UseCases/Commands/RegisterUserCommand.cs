using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ProjectDefense.Application.UseCases.Commands
{
    public record RegisterUserCommand(string Email, string Password, string Role) : IRequest<IdentityResult>;
}