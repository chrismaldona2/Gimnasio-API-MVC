﻿using Core.Entidades;
using Newtonsoft.Json;
using System.Text;
using WebApp.Models.Pago;
using WebApp.Services.Contracts;

namespace WebApp.Services
{
    public class PagoAPIService : APIService, IPagoAPIService
    {
        public PagoAPIService(HttpClient httpClient) : base(httpClient) { }

        public async Task<List<PagoModel>> ListaPagos()
        {
            List<PagoModel> listaPagos = new();
            var response = await _httpClient.GetAsync($"api/PagoService/Lista");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                listaPagos = JsonConvert.DeserializeObject<List<PagoModel>>(data);

                return listaPagos;
            }
            return listaPagos;
        }


        public async Task<APIResponse> RegistrarPagoAsync(PagoModel datosPago)
        {
            var response = await _httpClient.PostAsync($"api/PagoService/Registrar?IdCliente={datosPago.IdCliente}&IdMembresia={datosPago.IdMembresia}", null);
            var responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse
                {
                    Exitoso = true,
                    Mensaje = responseMessage
                };
            }
            else
            {
                return new APIResponse
                {
                    Exitoso = false,
                    Mensaje = responseMessage
                };
            }
        }

        public async Task<APIResponse> EliminarPagoAsync(int id)
        {

            var response = await _httpClient.DeleteAsync($"api/PagoService/Eliminar?Id={id}");
            var responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new APIResponse
                {
                    Exitoso = true,
                    Mensaje = responseMessage
                };
            }
            else
            {
                return new APIResponse
                {
                    Exitoso = false,
                    Mensaje = responseMessage
                };
            }
        }
    }
}