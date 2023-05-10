using Bpo;
using Jazz.Core;

namespace Jazz.Covenant.Service.BpoService
{
    public interface IBpoService
    {
        Task<consultaMargemConsignavelResponse?> ConsultMarginConsignableByCpf(string idCovenant, string cpf);
        Task<consultaMargemConsignavelResponse?> ConsultMarginConsignableByEnrollment(string idCovenant, string enrollment);
        Task<ApiResult<consultaMargemConsignavelResponse?>> ConsultMarginConsignableByCPFEnrollmentAutorization(string idCovenant, string enrollment,string cpf,string autorization);
        Task<consultaMargemCartaoResponse?> ConsultMarginCardCreditByCpf(string idCovenant, string cpf);
        Task<consultaMargemCartaoResponse?> ConsultMarginCardCrediByEnrollment(string idCovenant, string enrollment);
        Task<consultaMargemCartaoResponse?> ConsultMarginCardCreditByCPFEnrollmentAutorization(string idCovenant, string enrollment, string cpf, string autorization);

        Task<ApiResult<consultaMargemCartaoResponse>> ConsultMarginCreditCard(string idCovenant, string enrollment,
            string cpf, string autorization);
        Task<incluiEmprestimoResponse> MarginReserve(incluiEmprestimoRequest incluiEmprestimo, string identifierInEndoser);
        Task<incluiEmprestimoResponse?> EndosamentMargin(incluiEmprestimoRequest incluiEmprestimo, string identifierInEndoser);
        Task<excluiEmprestimoResponse> CancelMarginReserve(excluiEmprestimoRequest excluiEmprestimo, string identifierInEndoser);
        Task<excluiEmprestimoResponse> CancelMarginEndorsament(excluiEmprestimoRequest excluiEmprestimo, string identifierInEndoser);
    }
}
