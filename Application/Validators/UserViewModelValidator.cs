using Domain.AppDbContext;
using Domain.ViewModels;
using FluentValidation;
using Infrastructure.Customer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserView>
    {
        private readonly KayanDbContext _dbContext;

        public UserViewModelValidator(KayanDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .Must(BeUniqueEmail).WithMessage("Email already exists");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"[!@#$%^&*(),.?""':{}|<>]").WithMessage("Password must contain at least one special character");
        }

        private bool BeUniqueEmail(string email)
        {
            // Assuming _userRepo.ExistsByEmail returns true if email exists
            return !_dbContext.Users.Any(x => x.Email == email);
        }
    }
}
