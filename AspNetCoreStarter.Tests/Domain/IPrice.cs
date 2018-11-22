using AspNetCoreStarterPack.Cache;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Tests.Domain
{
    public class PriceValidator : AbstractValidator<IPrice>
    {
        public PriceValidator()
        {
            RuleFor(request => request.Asset).NotEmpty().WithMessage("Asset should be set");
            RuleFor(request => request.Value).NotEmpty().WithMessage("Value should be set");
        }
    }

    public interface IPrice : ICachedRessource
    {
        String Asset { get; }
        double Value { get; }
    }
}
