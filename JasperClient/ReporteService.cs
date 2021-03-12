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
        private string carpeta = "/Documentos/";
        private string archivo = "Reporte.pdf";

        public ReporteService()
        {
            cliente = ConsumirServicioExterno.Instance;
        }

        public async Task<ReporteModelDto> Listar()
        {
            var result = new ReporteModelDto();

            var peticion = new HttpRequestMessage(HttpMethod.Get, cliente.getURL() + "resources?type=reportUnit");

            try
            {
                HttpResponseMessage respuesta = await cliente.HacerPeticion(peticion);

                if(respuesta.IsSuccessStatusCode)
                {
                    result = await respuesta.Content.ReadAsAsync<ReporteModelDto>();

                    return devolverRespuesta(200, result, false,"");
                }

                return devolverRespuesta(402, null, false, respuesta.ReasonPhrase);
            }
            catch (Exception e)
            {
                return devolverRespuesta(402, null, true, e.Message);
            }
        }

        public async Task<RespuestaDto> ObtenerReporte(string uri)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, cliente.getURL() + uri);

            HttpResponseMessage respuesta = await cliente.HacerPeticion(peticion);

            var result = new RespuestaDto();

            if (respuesta.IsSuccessStatusCode)
            {
                try
                {
                    string ruta = obtenerDestino();

                    verificarExistenciaArchivo(ruta);

                    using (var fs = new FileStream(ruta, FileMode.CreateNew))
                    {
                        await respuesta.Content.CopyToAsync(fs);
                    }

                    return devolverRespuesta(200,ruta,false);
                    
                }
                catch (Exception e)
                {
                    return devolverRespuesta(402, e.Message, true);
                }
            }
            else
            {
                return devolverRespuesta(402, respuesta.ReasonPhrase, true);
            }
        }

        public void verificarExistenciaArchivo(string ruta)
        {
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }

            return;
        }

        public string obtenerDestino()
        {
            return System.Web.HttpContext.Current.Server.MapPath(string.Format(carpeta) + archivo);
        }

        private RespuestaDto devolverRespuesta(int estado, string datos, bool error)
        {
            RespuestaDto respuesta = new RespuestaDto();

            if(!error)
            {
                respuesta.Status = estado;
                respuesta.Data = datos;
            }
            else
            {
                respuesta.Status = estado;
                respuesta.Data = datos;
            }

            return respuesta;
        }

        private ReporteModelDto devolverRespuesta(int status, ReporteModelDto datos, bool error, string mensaje)
        {
            if(!error)
            {
                datos.Status = status;

                return datos;
            }
            else
            {
                datos.Status = status;
                datos.Error = mensaje;

                return datos;
            }
        }
    }
}
