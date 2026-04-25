using Autoberles;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using vizprog_beadando.db;

namespace vizprog_beadando
{
    /// <summary>
    /// Interaction logic for ModositAuto.xaml
    /// </summary>
    public partial class ModositAuto : Window
    {
        public Auto? auto;

        public ModositAuto(Auto? auto)
        {
            InitializeComponent();

            this.Title = auto == null ? "Adat hozzáadás" : $"Módosítás (Autó #{auto?.id})";
            this.auto = auto;

            submitBtn.Content = auto == null ? "Hozzáadás" : "Módosítás";

            if (auto != null)
            {
                marka.Text = auto.marka;
                tipus.Text = auto.tipus;
                berles_dij.Text = auto.berles_dij.ToString();
            }
        }

        private void modositClick(object sender, RoutedEventArgs e)
        {
            if (marka.Text == "")
            {
                MessageBox.Show("A márka mező nem lehet üres!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (tipus.Text == "")
            {
                MessageBox.Show("A típus mező nem lehet üres!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(berles_dij.Text, out int berlesDij))
            {
                MessageBox.Show("A bérlés díjának egy egész számnak kell lennie!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (berlesDij < 0)
            {
                MessageBox.Show("A bérlés díjának nem lehet negatívnak lennie!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            short id = this.auto != null ? this.auto.id : (short)(-1);
            this.auto = new Auto()
            {
                id = id,
                marka = marka.Text,
                tipus = tipus.Text,
                berles_dij = berlesDij
            };

            this.Close();
        }

        private void megseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
