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

        private readonly HttpClient client; //variable de retorno de instancia singleton

        private string user = "jasperadmin";
        private string password = "jasperadmin";

        private string URL = "http://localhost:8080/jasperserver/rest_v2/"; //URL base del servidor para peticiones

        private ConsumirServicioExterno()
        {
            client = new HttpClient();

            AgregarCabeceras();
            AgregarAutenticacion();
        }

        private void AgregarAutenticacion()
        {
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
        }

        private void AgregarCabeceras()
        {
            client.DefaultRequestHeaders.Add("Accept","application/json");
            client.DefaultRequestHeaders.Add("Connection","keep-alive");
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
            
            respuesta = await client.SendAsync(parametros);

            return respuesta; 
        }
    }
}
