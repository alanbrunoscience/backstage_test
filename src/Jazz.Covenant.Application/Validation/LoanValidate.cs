using FluentValidation;
using Jazz.Common;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Application.Validation;

public class LoanValidate
{
  public static LoanMarginReserveCommandValidator GetLoanReservetValidate()
  {

      return new LoanMarginReserveCommandValidator();

  }
  public static LoanMarginEnorsermentCommandValidator GetLoanEndorsamentValidate()
  {

      return new LoanMarginEnorsermentCommandValidator();

  }
}

public class LoanMarginReserveCommandValidator : AbstractValidator<MarginReserveCommand>
{

  public LoanMarginReserveCommandValidator()
  {
    RuleFor(cmd => cmd.Payload.TaxId)
      .NotEmpty()
      .SetValidator(Cpf.GetValidator())
      .WithName(nameof(MarginReservePayload.TaxId))
      .OverridePropertyName(nameof(MarginReservePayload.TaxId));
    RuleFor(cmd => cmd.Payload.Enrollment).NotEmpty();
    RuleFor(cmd => cmd.Payload.Name).NotEmpty();
    RuleFor(cmd => cmd.Payload.ContractValue).NotEmpty();
    RuleFor(cmd => cmd.Payload.ValueInstallment).NotEmpty();
    RuleFor(cmd => cmd.Payload.AmountReleased).NotEmpty();
    RuleFor(cmd => cmd.Payload.NumberOfInstallments).NotEmpty();
    RuleFor(cmd => cmd.Payload.ContractNumber).NotEmpty();
    RuleFor(cmd => cmd.Payload.PortableIdentifierNumberReserveCovenant).NotEmpty();
    RuleFor(cmd => cmd.Payload.BankAccount).NotEmpty();
    RuleFor(cmd => cmd.Payload.AgencyAccount).NotEmpty();
    RuleFor(cmd => cmd.Payload.Account).NotEmpty();
    RuleFor(cmd => cmd.Payload.InterestRate).NotEmpty();
    RuleFor(cmd => cmd.Payload.IOFValue).NotEmpty();
    RuleFor(cmd => cmd.Payload.CETValue).NotEmpty();
    RuleFor(cmd => cmd.Payload.BirthDate).NotEmpty();
    RuleFor(cmd => cmd.Payload.ValueInstallmentPortability).NotEmpty();
    RuleFor(cmd => cmd.Payload.ValueInstallmentRefinancing).NotEmpty();
    RuleFor(cmd => cmd.Payload.ContractDuration).NotEmpty();
    RuleFor(cmd => cmd.Payload.ExpirationDay).NotEmpty();
    RuleFor(cmd => cmd.Payload.ContractEndDate).NotEmpty();
    RuleFor(x => x.IdCovenant).SetValidator(GuidValidate.GetValidations());
  }


}

public class LoanMarginEnorsermentCommandValidator : AbstractValidator<EndorsementMarginCommand>
{
  public LoanMarginEnorsermentCommandValidator()
  {
    RuleFor(cmd => cmd.Payload.TaxId)
      .NotEmpty()
      .SetValidator(Cpf.GetValidator())
      .WithName(nameof(EndorsementMarginCommand.Payload.TaxId))
      .OverridePropertyName(nameof(EndorsementMarginCommand.Payload.TaxId));

    RuleFor(cmd => cmd.Payload.Enrollment).NotEmpty();
    RuleFor(cmd => cmd.Payload.Name).NotEmpty();
    RuleFor(cmd => cmd.Payload.ContractValue).NotEmpty();
    RuleFor(cmd => cmd.Payload.ValueInstallment).NotEmpty();
    RuleFor(cmd => cmd.Payload.AmountReleased).NotEmpty();
    RuleFor(cmd => cmd.Payload.NumberOfInstallments).NotEmpty();
    RuleFor(cmd => cmd.Payload.ContractNumber).NotEmpty();
    RuleFor(cmd => cmd.Payload.PortableIdentifierNumberReserveCovenant).NotEmpty();
    RuleFor(cmd => cmd.Payload.BankAccount).NotEmpty();
    RuleFor(cmd => cmd.Payload.AgencyAccount).NotEmpty();
    RuleFor(cmd => cmd.Payload.Account).NotEmpty();
    RuleFor(cmd => cmd.Payload.ProductType).IsInEnum();
    RuleFor(cmd => cmd.Payload.InterestRate).NotEmpty();
    RuleFor(cmd => cmd.Payload.IOFValue).NotEmpty();
    RuleFor(cmd => cmd.Payload.CETValue).NotEmpty();
    RuleFor(cmd => cmd.Payload.BirthDate).NotEmpty();
    RuleFor(cmd => cmd.Payload.ValueInstallmentPortability).NotEmpty();
    RuleFor(cmd => cmd.Payload.ValueInstallmentRefinancing).NotEmpty();
    RuleFor(cmd => cmd.Payload.ContractDuration).NotEmpty();
    RuleFor(cmd => cmd.Payload.ExpirationDay).NotEmpty();
    RuleFor(cmd => cmd.Payload.ContractEndDate).NotEmpty();
    RuleFor(x => x.IdCovenant).SetValidator(GuidValidate.GetValidations());
  }
}
