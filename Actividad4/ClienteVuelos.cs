using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Actividad4
{
    public class ClienteVuelos
    {
        HttpClient cliente = new HttpClient();
        
        public ClienteVuelos()
        {
            cliente.BaseAddress = new Uri("http://vuelos.itesrc.net");
        }
        public async void Agregar(DatosVuelos vuelos)
        {

            if (vuelos.Estado == "A TIEMPO" || vuelos.Estado == "ABORDANDO" || vuelos.Estado == "CANCELADO" || vuelos.Estado == "RETRASADO")
            {
                var json = JsonConvert.SerializeObject(vuelos);
                var result = await cliente.PostAsync("/Tablero", new StringContent(json, Encoding.UTF8, "application/json"));
                result.EnsureSuccessStatusCode();             
            }
            else
            {
                MessageBox.Show("El vuelo debe tener un estado");
            }
        }
        public async void Editar(DatosVuelos vuelos)
        {
            if (vuelos.Estado == "A TIEMPO" || vuelos.Estado == "ABORDANDO" || vuelos.Estado == "CANCELADO" || vuelos.Estado == "RETRASADO")
            {
                var json = JsonConvert.SerializeObject(vuelos);
                var result = await cliente.PutAsync("/Tablero", new StringContent(json, Encoding.UTF8, "application/json"));
                result.EnsureSuccessStatusCode();
            }
            else
            {
                MessageBox.Show("El vuelo debe tener un estado");
            }
        }
        public async void Eliminar(DatosVuelos vuelos)
        {
            var json = JsonConvert.SerializeObject(vuelos);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, "/Tablero");
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await cliente.SendAsync(message);
            result.EnsureSuccessStatusCode();
        }

        public delegate void Invokar();
        public event Invokar AlInvokar;
        public IEnumerable<DatosVuelos> model { get; set; }
        public async void Get()
        {            
            var client = new HttpClient();
            var result = await client.GetAsync("http://vuelos.itesrc.net/Tablero");
            if (result.IsSuccessStatusCode)
            {
                var jsonString = await result.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<IEnumerable<DatosVuelos>>(jsonString);
                AlInvokar?.Invoke();
            }
        }
    }
}
