using Common;
using JasperClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Controllers
{
    public class JasperController : ApiController
    {
        private readonly IReporteService service;

        public JasperController()
        {
            this.service = new ReporteService();
        }

        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            var result = new ReporteModelDto();

            try
            {
                result = await service.Listar();
                result.Status = 200;
            }
            catch (Exception e)
            {
                result.Status = e.HResult;
                result.Error = e.Message;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> DownloadFile(int id)
        {
            var result = new RespuestaDto();

            string url = "reports/Demo_Studio/Clientes.pdf?Registros=10&Pais=Mexico";

            try
            {
                result = await service.ObtenerReporte(url);
                result.Status = 200;
            }
            catch (Exception e)
            {
                result.Status = e.HResult;
                result.Data = e.Message;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }
    }
}
