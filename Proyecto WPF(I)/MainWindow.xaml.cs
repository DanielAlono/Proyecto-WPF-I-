using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Proyecto_WPF_I_
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Numero random y una lista de números para no repetir
        Random generador = new Random();
        List<int> listaNumeros;

        //Mi lista de películas para gestionar y para jugar
        private ObservableCollection<Pelicula> collection;
        private ObservableCollection<Pelicula> jugarCollection;
        Pelicula pelicula;
        //Número de lista donde nos encontramos y la puntuación del jugador
        int numeroLista, puntuacion;

        public MainWindow()
        {
            InitializeComponent();
            //Inicializamos la lista de números y la colección de películas
            numeroLista = 0;
            collection = new ObservableCollection<Pelicula>();

            //Inicializamos los componentes de la ventana pertinentes
            nombresComboBox();
            facilRadioButton.IsChecked = true; //Fácil por defecto
            peliculasListBox.DataContext = collection;
        }

        private void añadirButton_Click(object sender, RoutedEventArgs e)
        {
            //Iniciamos la variable película y le asignamos valor a todos sus campos
            pelicula = new Pelicula();
            pelicula.Titulo = tituloTextBox.Text;
            pelicula.Imagen = imagenTextBox.Text;
            pelicula.Pista = pistaTextBox.Text;
            pelicula.Dificultad = dificultad();
            pelicula.Genero = opcionesComboBox.Text;
            pelicula.Acertada = false;
            collection.Add(pelicula);

            //Mostramos un pequeño mensaje y reiniciamos las casillas a valores nulos o vacíos
            MessageBox.Show("Película insertada con éxito", "Insertar", MessageBoxButton.OK, MessageBoxImage.Information);
            reiniciarCasillas();
        }

        private string dificultad()
        {
            //Comprobamos qué botón está seleccionado y devolvemos la cadena
            if ((bool)facilRadioButton.IsChecked)
            {
                return "Facil";
            }
            else if ((bool)normalRadioButton.IsChecked)
            {
                return "Normal";
            }
            else if ((bool)dificilRadioButton.IsChecked)
            {
                return "Dificil";
            }
            return null;
        }
        private void eliminarButton_Click(object sender, RoutedEventArgs e)
        {
            collection.Remove((Pelicula)peliculasListBox.SelectedItem);
        }

        private void reiniciarCasillas()
        {
            tituloTextBox.Text = "";
            imagenTextBox.Text = "";
            pistaTextBox.Text = "";
            facilRadioButton.IsChecked = true;
            opcionesComboBox.SelectedIndex = 0;
        }
        private void nombresComboBox()
        {
            List<String> generos = new List<string>() { "Comedia", "Drama", "Acción", "Terror", "Ciencia Ficción" };
            opcionesComboBox.ItemsSource = generos;
            opcionesComboBox.SelectedIndex = 0; //dejamos el primer item de la lista seleccionado
        }

        private void cargarJSONButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string json = File.OpenText(openFileDialog.FileName).ReadToEnd();
                ObservableCollection<Pelicula> peliculas = JsonConvert.DeserializeObject<ObservableCollection<Pelicula>>(json);

                foreach (Pelicula p in peliculas)
                {
                    collection.Add(p);
                }
            }
        }

        private void guardarJSONButton_Click(object sender, RoutedEventArgs e)
        {
            string peliculasJson = JsonConvert.SerializeObject(collection);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter= "*.json|*.json|*.txt|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, peliculasJson);
            }
        }

        private void examinarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                imagenTextBox.Text = openFileDialog.FileName;
            }
        }

        private void nuevaPartidaButton_Click(object sender, RoutedEventArgs e)
        {
            //Compruebo si tenemos al menos 5 películas, de lo contrario enseñamos un mensaje
            if (collection.Count < 5)
            {
                MessageBox.Show("La lista de películas no puede ser inferior a 5", "Peliculas insuficientes", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                //Iniciamos puntuación y asignamos el valor en la ventana
                puntuacion = 0;
                puntuacionTextBlock.Text = puntuacion.ToString();

                //Inicializamos la lista de números para que no se repitan y añadimos el -1
                listaNumeros = new List<int>();
                listaNumeros.Add(-1);

                jugarCollection = new ObservableCollection<Pelicula>();
                //Añadimos 5 películas
                //Así podremos añadir aleatoriamente películas sin que se repitan
                jugarCollection.Add(collection[numeroAleatorio()]);
                jugarCollection.Add(collection[numeroAleatorio()]);
                jugarCollection.Add(collection[numeroAleatorio()]);
                jugarCollection.Add(collection[numeroAleatorio()]);
                jugarCollection.Add(collection[numeroAleatorio()]);
                //Asignamos al panel la primera película
                verPeliculaGrid.DataContext = jugarCollection[0];

                actualPeliculaTextBlock.Text = "1";
                totalPeliculasTextBlock.Text = jugarCollection.Count.ToString();
            }
        }

        private int numeroAleatorio()
        {
            //Comprobamos que no esté en nuestra lista de números y devolvemos su valor
            int numero = -1;
            while (listaNumeros.Contains(numero))
            {
                numero = generador.Next(0, collection.Count);
            }
            listaNumeros.Add(numero);
            return numero;
        }

        private void actualizarDatos(Pelicula pelicula)
        {
            //Método para llamar al cambiar de película en la venta a jugar cada vez que cambiemos de película

            /*Quizá debería haber cambiado la comprobación al revés para poder vincularla con un simple Binding 
             * en la ventana, aunque como no era demasiado código, lo he dejado así. De la otra manera sería: 
             * IsEnabled = {Binding Path=Acertada} sobraba. Lo que pasa que los bool son contrarios como lo tengo*/
            if (pelicula.Acertada) validarButton.IsEnabled = false;
            else validarButton.IsEnabled = true;

            verPeliculaGrid.DataContext = pelicula;
            actualPeliculaTextBlock.Text = (numeroLista +1).ToString();
            totalPeliculasTextBlock.Text = jugarCollection.Count.ToString();
            tituloPeliculaTextBox.Text = "";
            puntuacionTextBlock.Text = puntuacion.ToString();
        }

        private void retrocederFlechaImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(numeroLista > 0)
            {
                numeroLista -= 1;
                actualizarDatos(jugarCollection[numeroLista]);
            }
        }

        private void validarButton_Click(object sender, RoutedEventArgs e)
        {
            Pelicula p = jugarCollection[numeroLista];
            //Comprobamos todo en minúscula por si lo introduce con mayúsculas o sin ellas
            if (tituloPeliculaTextBox.Text.ToLower() == p.Titulo.ToLower())
            {
                //La película ha sido acertada
                jugarCollection[numeroLista].Acertada = true;
                //Sumamos los puntos
                switch (p.Dificultad)
                {
                    case "Facil":
                        if ((bool)verPistaCheckBox.IsChecked) puntuacion += 3;
                        else puntuacion += 5;
                        break;
                    case "Normal":
                        if ((bool)verPistaCheckBox.IsChecked) puntuacion += 5;
                        else puntuacion += 10;
                        break;
                    case "Dificil":
                        if ((bool)verPistaCheckBox.IsChecked) puntuacion += 10;
                        else puntuacion += 20;
                        break;
                }
            }
            actualizarDatos(p);
        }

        private void avanzarFlechaImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(numeroLista < jugarCollection.Count - 1)
            {
                numeroLista += 1;
                actualizarDatos(jugarCollection[numeroLista]);
            }
        }
    }
}
