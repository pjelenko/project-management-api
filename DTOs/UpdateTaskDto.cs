using FluentValidation;

namespace ProjectManagement.DTOs
{
    public record UpdateTaskDto(string Name, int? Points, int? UserId, Models.Enums.TaskStatus? Status);

    public class UpdateTaskDtoValidator: AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator() 
        {
            RuleFor(x => x.Name).Must(x => x != "");
            RuleFor(x => x.Points).Must(x => x == null || x > 0);
            RuleFor(x => x.UserId).Must(x => x == null || x >= 0);
        }
    }

}
