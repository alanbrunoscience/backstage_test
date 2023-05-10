

using FluentValidation;
using Jazz.Common;
using Jazz.Covenant.Application.RequestHandlers;
using Jazz.Covenant.Domain.Enums;

namespace Jazz.Covenant.Application.Validation;

public class CardCreditValidate
{
 public static CardCreditMarginReserve GetValidateCardCreditMarginReserve()
 {
   return new CardCreditMarginReserve();
 }
 public static CardCreditEndorsament GetCardCreditEndorsamentValidate()
 {

     return new CardCreditEndorsament();

 }
}

public class CardCreditMarginReserve :AbstractValidator<MarginReserveCommand>
{
  public CardCreditMarginReserve()
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
    RuleFor(cmd => cmd.Payload.ContractNumber).NotEmpty();
    RuleFor(cmd => cmd.Payload.InterestRate).NotEmpty();
    RuleFor(cmd => cmd.Payload.CETValue).NotEmpty();
    RuleFor(cmd => cmd.Payload.BirthDate).NotEmpty();
    RuleFor(cmd => cmd.Payload.NumberOfInstallments).NotEmpty();
    RuleFor(x => x.IdCovenant).SetValidator(GuidValidate.GetValidations());
  }
}

public class CardCreditEndorsament : AbstractValidator<EndorsementMarginCommand>
{

    public CardCreditEndorsament()
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
      RuleFor(cmd => cmd.Payload.ContractNumber).NotEmpty();
      RuleFor(cmd => cmd.Payload.ProductType).IsInEnum();
      RuleFor(cmd => cmd.Payload.InterestRate).NotEmpty();
      RuleFor(cmd => cmd.Payload.CETValue).NotEmpty();
      RuleFor(cmd => cmd.Payload.NumberOfInstallments).NotEmpty();
      RuleFor(cmd => cmd.Payload.BirthDate).NotEmpty();
      RuleFor(x => x.IdCovenant).SetValidator(GuidValidate.GetValidations());
    }


  }


