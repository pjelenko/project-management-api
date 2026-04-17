using FluentValidation;

namespace ProjectManagement.DTOs
{
    public record CreateProjectDto(string Name, int OwnerId);

    public class CreateProjectDtoValidator: AbstractValidator<CreateProjectDto>
    {
        public CreateProjectDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
