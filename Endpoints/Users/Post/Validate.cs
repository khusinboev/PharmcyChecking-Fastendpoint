using FluentValidation;

namespace PharmcyChecking.Endpoints.Users.Post
{
    public class Validate : Validator<Request>
    {
        public Validate()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Code)
                .NotEmpty()
                .NotNull();
        }
    }
}
