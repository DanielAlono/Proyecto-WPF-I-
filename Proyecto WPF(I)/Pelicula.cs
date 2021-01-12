using System.ComponentModel;

namespace Proyecto_WPF_I_
{

    class Pelicula : INotifyPropertyChanged
    {
        private string _titulo;

        public string Titulo
        {
            get { return this._titulo; }
            set
            {
                if (this._titulo != value)
                {
                    this._titulo = value;
                    this.NotifyPropertyChanged("Titulo");
                }
            }
        }

        private string _imagen;

        public string Imagen
        {
            get { return this._imagen; }
            set
            {
                if (this._imagen != value)
                {
                    this._imagen = value;
                    this.NotifyPropertyChanged("Imagen");
                }
            }
        }

        private string _pista;

        public string Pista
        {
            get { return this._pista; }
            set
            {
                if (this._pista != value)
                {
                    this._pista = value;
                    this.NotifyPropertyChanged("Pista");
                }
            }
        }

        private string _dificultad;

        public string Dificultad
        {
            get { return this._dificultad; }
            set
            {
                if (this._dificultad != value)
                {
                    this._dificultad = value;
                    this.NotifyPropertyChanged("Dificultad");
                }
            }
        }

        private string _genero;

        public string Genero
        {
            get { return this._genero; }
            set
            {
                if (this._genero != value)
                {
                    this._genero = value;
                    this.NotifyPropertyChanged("Genero");
                }
            }
        }

        public Pelicula() { }

        public Pelicula(string titulo, string imagen, string pista, string dificultad, string genero)
        {
            Titulo = titulo;
            Imagen = imagen;
            Pista = pista;
            Dificultad = dificultad;
            Genero = genero;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
