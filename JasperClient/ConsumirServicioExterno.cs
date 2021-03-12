using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JasperClient
{
    public class ConsumirServicioExterno
    {
        private static ConsumirServicioExterno instance = null; 

        private readonly HttpClient cliente;

        private string user = "jasperadmin";
        private string password = "jasperadmin";

        private string URL = "http://localhost:8080/jasperserver/rest_v2/";

        private ConsumirServicioExterno()
        {
            cliente = new HttpClient();

            AgregarCabeceras();
            AgregarAutenticacion();
        }

        private void AgregarAutenticacion()
        {
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
        }

        private void AgregarCabeceras()
        {
            cliente.DefaultRequestHeaders.Add("Accept","application/json");
            cliente.DefaultRequestHeaders.Add("Connection","keep-alive");
        }

        public static ConsumirServicioExterno Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConsumirServicioExterno();
                }

                return instance;
            }
        }

        public string getURL()
        {
            return URL;
        }

        public async Task<HttpResponseMessage> HacerPeticion(HttpRequestMessage parametros)
        {
            HttpResponseMessage respuesta;
            
            respuesta = await cliente.SendAsync(parametros);

            return respuesta; 
        }
    }
}
