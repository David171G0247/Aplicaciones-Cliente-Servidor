using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Threading;
using System.IO;
using System.Web;

namespace Actividad3
{
    public class Servidor
    {
        HttpListener listener;
        public Valores valores { get; set; } = new Valores();
        Dispatcher dispatcher;

        public Servidor()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:8080/actividad3/");
            listener.Start();
            dispatcher = Dispatcher.CurrentDispatcher;
            listener.BeginGetContext(Recibir, null);
        }

        private void Recibir(IAsyncResult ar)
        {
            var context = listener.EndGetContext(ar);
            listener.BeginGetContext(Recibir, null);
            var url = context.Request.Url.LocalPath;
            if (url.EndsWith("/"))
            {
                url = url.Remove(url.Length - 1, 1);
            }

            if (context.Request.HttpMethod == "GET" && url == "/actividad3")
            {
                var index = File.ReadAllBytes("index.html");
                context.Response.ContentType = "text/html";
                context.Response.OutputStream.Write(index, 0, index.Length);
                context.Response.StatusCode = 200;
                context.Response.OutputStream.Close();
            }
            else if (context.Request.HttpMethod == "POST" && url == "/actividad3")
            {
                StreamReader stream = new StreamReader(context.Request.InputStream);
                string variables = stream.ReadToEnd();

                var datos = HttpUtility.ParseQueryString(variables);

                Incrementar(datos["puntos"]);

                context.Response.StatusCode = 200;
                context.Response.Redirect("/actividad3/");
            }
            else if (context.Request.HttpMethod == "GET" && context.Request.Url.LocalPath == "/actividad3/actualizar")
            {
                if (context.Request.QueryString["Nombre1"] != null && context.Request.QueryString["Nombre2"] != null)
                {
                    var nombre1 = context.Request.QueryString["Nombre1"];
                    var nombre2 = context.Request.QueryString["Nombre2"];
                    var color1 = context.Request.QueryString["Color1"];
                    var color2 = context.Request.QueryString["Color2"];
                    Actualizar(nombre1, nombre2, color1, color2);
                    context.Response.StatusCode = 200;
                    context.Response.Redirect("/actividad3/");
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Escriba los nombres de ambos boxeadores";
                }
            }
            else
            {
                context.Response.StatusCode = 404;
            }
            context.Response.Close();
        }

        public void Incrementar(string puntos)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                if (puntos == "Puntos1")
                {
                    valores.Puntos1++;
                }
                else if (puntos == "Puntos2")
                {
                    valores.Puntos2++;
                }
                else if(puntos == "Round")
                {
                    valores.Round++;
                }
            }));
        }
        public void Actualizar(string Nombre1, string Nombre2, string Color1, string Color2)
        {

            dispatcher.BeginInvoke(new Action(() =>
            {
                valores.Nombre1 = Nombre1;
                valores.Nombre2 = Nombre2;
                valores.Color1 = Color1;
                valores.Color2 = Color2;
            }));
        }
    }
}
