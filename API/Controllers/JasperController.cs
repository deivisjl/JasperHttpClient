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

            result = await service.Listar();

            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> DownloadFile(int id)
        {
            var result = new RespuestaDto();

            string url = "reports/Demo_Studio/Clientes.pdf?Registros=10&Pais=Mexico";

            result = await service.ObtenerReporte(url);

            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }
    }
}
