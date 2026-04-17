using FluentValidation;

namespace ProjectManagement.DTOs
{
    public record RegisterUserDto(string FullName, string Username, string Password, string Email);

    public class RegisterDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).MinimumLength(6);
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
