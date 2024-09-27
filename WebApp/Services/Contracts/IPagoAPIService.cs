using WebApp.Models.Pago;

namespace WebApp.Services.Contracts
{
    public interface IPagoAPIService
    {
        Task<List<PagoModel>> ListaPagos();
        Task<APIResponse> RegistrarPagoAsync(PagoModel datosPago);
        Task<APIResponse> EliminarPagoAsync(int id);
        Task<PagoModel> BuscarPagoPorId(int id);
    }
}
