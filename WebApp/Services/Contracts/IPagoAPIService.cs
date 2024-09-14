using WebApp.Models.Pago;

namespace WebApp.Services.Contracts
{
    public interface IPagoAPIService
    {
        Task<List<PagoModel>> ListaPagos();
    }
}
