using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Application.Validation
{

    public class GuidValidate
    {
        public static GuidValidation GetValidations() => new();
    }

    public class GuidValidation:AbstractValidator<string>
    {
        public GuidValidation()
        {

            RuleFor(c => c).NotEmpty()
                .Must(e => Guid.TryParse(e, out var outendosament)
            ).WithMessage("Invalid Guid");
        }
    }
}
