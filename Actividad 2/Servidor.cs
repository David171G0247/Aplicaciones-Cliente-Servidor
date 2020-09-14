using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.ComponentModel;

namespace Actividad_2
{
    public class Servidor: INotifyPropertyChanged
    {
        HttpListener listener;
        Dispatcher dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            };
        }
        public string Texto { get; set; }
        public string Color { get; set; }
        public Servidor()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:80/actividad2/");
            listener.Start();
            listener.BeginGetContext(OnRequest, null);
        }
        private void OnRequest(IAsyncResult ar)
        {
            var context = listener.EndGetContext(ar);
            listener.BeginGetContext(OnRequest, null);

            if (context.Request.Url.LocalPath == "/actividad2/" || context.Request.Url.LocalPath == "/actividad2")
            {
                var buffer = File.ReadAllBytes("Index.html");
                context.Response.ContentType = "text/html";
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();

                context.Response.StatusCode = 200;
            }
            else if(context.Request.Url.LocalPath == "/actividad2/cambiartextoget" && context.Request.HttpMethod == "GET")
            {
                if (context.Request.QueryString["texto"] != null)
                {
                    var texto = context.Request.QueryString["texto"];
                    var color = context.Request.QueryString["color"];
                    Actualizar(texto, color);
                    context.Response.StatusCode = 200;
                    context.Response.Redirect("/actividad2/");
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Olvidaste incluir el texto";
                }
            }
            else
            {
                context.Response.StatusCode = 404;
            }
            context.Response.Close();
        }
        private void Actualizar (string texto, string color)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                Texto = texto;
                Color = color;
            }));
            //OnPropertyChanged("Texto");
            //OnPropertyChanged("Color");


        }
    }
}
