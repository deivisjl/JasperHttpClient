using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace JasperClient
{
    public interface IReporteService
    {
        Task<ReporteModelDto> Listar();
        Task<RespuestaDto> ObtenerReporte(string uri);
    }

    public class ReporteService : IReporteService
    {
        private readonly ConsumirServicioExterno cliente;

        public ReporteService()
        {
            cliente = ConsumirServicioExterno.Instance;
        }

        public async Task<ReporteModelDto> Listar()
        {
            var result = new ReporteModelDto();

            var peticion = new HttpRequestMessage(HttpMethod.Get, cliente.getURL() + "resources?type=reportUnit");

            HttpResponseMessage respuesta = await cliente.HacerPeticion(peticion);

            if (respuesta.IsSuccessStatusCode)
            {
                using (var resp = respuesta)
                {
                    result = await resp.Content.ReadAsAsync<ReporteModelDto>();
                }
            }
            else
            {
                throw new Exception(respuesta.ReasonPhrase);                
            }

            return result;

        }

        public async Task<RespuestaDto> ObtenerReporte(string uri)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, cliente.getURL() + uri);

            HttpResponseMessage respuesta = await cliente.HacerPeticion(peticion);

            var result = new RespuestaDto();

            if (respuesta.IsSuccessStatusCode)
            {
                string ruta = System.Web.HttpContext.Current.Server.MapPath(string.Format("/Documentos/") + "Reporte.pdf");

                try
                {
                    if (File.Exists(ruta))
                    {
                        File.Delete(ruta);
                    }

                    using (var fs = new FileStream(ruta, FileMode.CreateNew))
                    {
                        await respuesta.Content.CopyToAsync(fs);
                    }

                    result.Status = 200;
                    result.Data = ruta;
                }
                catch (Exception e)
                {
                    result.Status = 402;
                    result.Data = e.Message;
                }

                return result;
            }
            else
            {
                result.Status = 402;
                result.Data = respuesta.ReasonPhrase;

                return result;
            }
        }
    }
}
