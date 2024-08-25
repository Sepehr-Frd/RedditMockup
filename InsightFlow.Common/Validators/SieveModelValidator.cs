using FluentValidation;
using Sieve.Models;

namespace InsightFlow.Common.Validators;

public class SieveModelValidator : AbstractValidator<SieveModel>
{
    public SieveModelValidator()
    {
        RuleFor(sieveModel => sieveModel.PageSize)
            .LessThanOrEqualTo(100);
    }
}