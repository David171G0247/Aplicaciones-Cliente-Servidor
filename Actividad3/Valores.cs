using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actividad3
{
    public class Valores : INotifyPropertyChanged
    {
        private int puntos1;
        public int Puntos1
        {
            get { return puntos1; }
            set
            {
                puntos1 = value;
                OnPropertyChanged("Puntos1");
            }
        }
        private int puntos2;

        public int Puntos2
        {
            get { return puntos2; }
            set
            {
                puntos2 = value;
                OnPropertyChanged("Puntos2");
            }
        }

        private int round;
        public int Round
        {
            get { return round; }
            set 
            { 
                round = value;
                OnPropertyChanged("Round");
            }
        }

        private string nombre1;
        public string Nombre1
        {
            get { return nombre1; }
            set { 
                nombre1 = value;
                OnPropertyChanged("nombre1");
            }
        }

        private string nombre2;
        public string Nombre2
        {
            get { return nombre2; }
            set { 
                nombre2 = value;
                OnPropertyChanged("nombre2");
            }
        }
        private string color1;

        public string Color1
        {
            get { return color1; }
            set { color1 = value;
                OnPropertyChanged("color1");
            }
        }
        private string color2;

        public string Color2
        {
            get { return color2; }
            set { color2 = value;
                OnPropertyChanged("color2");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propiedad)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
        }
    }
}
