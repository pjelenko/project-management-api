using FluentValidation;

namespace ProjectManagement.DTOs
{
    public record CreateTaskDto(string Name, int Points, int ProjectId, int? UserId);

    public class CreateTaskDtoValidator: AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator() 
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Points).GreaterThanOrEqualTo(1);
        }
    }
}