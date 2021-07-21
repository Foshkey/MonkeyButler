using FluentValidation;
using MonkeyButler.Abstractions.Business.Models.FreeCompanySearch;

namespace MonkeyButler.Business.Validators.FreeCompanySearch
{
    internal class FreeCompanySearchValidator : AbstractValidator<FreeCompanySearchCriteria>
    {
        public FreeCompanySearchValidator()
        {
            RuleFor(x => x.Query)
                .NotEmpty();
        }
    }
}
