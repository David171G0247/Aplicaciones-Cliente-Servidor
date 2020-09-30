using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Actividad4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatosVuelos datos = new DatosVuelos();
        ClienteVuelos cliente = new ClienteVuelos();
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = datos;
            cliente.Get();
            cliente.AlInvokar += Cliente_AlInvokar;
            cmbEstado.Items.Add("A TIEMPO");
            cmbEstado.Items.Add("ABORDANDO");
            cmbEstado.Items.Add("CANCELADO");
            cmbEstado.Items.Add("RETRASADO");
            //

            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();
            Editar.IsEnabled = false;
            Eliminar.IsEnabled = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            cliente.Get();
        }

        private void Cliente_AlInvokar()
        {
            dtgDatos.ItemsSource = cliente.model;
        }


        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (txtDestino.Text != "" || txtHora.Text != "" || txtVuelo.Text != "")
            {
                try
                {
                    datos.Hora = txtHora.Text;
                    datos.Destino = txtDestino.Text;
                    datos.Vuelo = txtVuelo.Text;
                    datos.Estado = cmbEstado.Text;
                    cliente.Agregar(datos);
                    timer.Start();
                    txtDestino.Clear();
                    txtHora.Clear();
                    txtVuelo.Clear();
                    cmbEstado.Text = "";
                    Editar.IsEnabled = false;
                    Eliminar.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Llene los campos para agregar un nuevo vuelo");
            }
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDatos.SelectedIndex != -1)
            {
                try
                {
                    datos.Hora = txtHora.Text;
                    datos.Destino = txtDestino.Text;
                    datos.Vuelo = txtVuelo.Text;
                    datos.Estado = cmbEstado.Text;
                    cliente.Editar(datos);
                    cliente.Get();
                    timer.Start();
                    txtDestino.Clear();
                    txtHora.Clear();
                    txtVuelo.Clear();
                    cmbEstado.Text = "";
                    dtgDatos.SelectedItem = null;
                    Editar.IsEnabled = false;
                    Eliminar.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Es necesario elegir un elemento para poder editarlo.");
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDatos.SelectedIndex != -1)
            {              
                try
                {   
                    DatosVuelos datosEliminar = dtgDatos.SelectedItem as DatosVuelos; 
                    cliente.Eliminar(datosEliminar);
                    timer.Start();
                    txtDestino.Clear(); 
                    txtHora.Clear();
                    txtVuelo.Clear(); 
                    cmbEstado.Text = "";
                    dtgDatos.SelectedItem = null;
                    Editar.IsEnabled = false;
                    Eliminar.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                timer.Start();
            }
            else
            {
                MessageBox.Show("Es necesario elegir un elemento para poder eliminarlo." );
            }
        }
        private void dtgDatos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgDatos.SelectedItem != null)
            {
                Editar.IsEnabled = true;
                Eliminar.IsEnabled = true;
                timer.Stop();
                datos = dtgDatos.SelectedItem as DatosVuelos;
                txtHora.Text = datos.Hora;
                txtDestino.Text = datos.Destino;
                txtVuelo.Text = datos.Vuelo;
                cmbEstado.Text = datos.Estado;
            }
        }
    }
}
