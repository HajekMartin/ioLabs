using FluentValidation;
using ioLabs.Models;

namespace ioLabs.Validators
{
    public class DataModelValidator : AbstractValidator<DataModel>
    {
        public DataModelValidator()
        {
            RuleFor(x => x.Request).NotEmpty().WithMessage("The message content cannot be empty");
        }
    }
}
