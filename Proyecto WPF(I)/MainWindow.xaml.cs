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
        private ObservableCollection<Pelicula> collection;
        Pelicula pelicula;
        public MainWindow()
        {
            InitializeComponent();
            collection = new ObservableCollection<Pelicula>();
            nombresComboBox();
            facilRadioButton.IsChecked = true; //Fácil por defecto
            peliculasListBox.DataContext = collection;
        }

        private void añadirButton_Click(object sender, RoutedEventArgs e)
        {
            pelicula = new Pelicula();
            pelicula.Titulo = tituloTextBox.Text;
            pelicula.Imagen = imagenTextBox.Text;
            pelicula.Pista = pistaTextBox.Text;
            pelicula.Dificultad = dificultad();
            pelicula.Genero = opcionesComboBox.Text;
            collection.Add(pelicula);
            reiniciarCasillas();
        }
        private string dificultad()
        {
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
    }
}
