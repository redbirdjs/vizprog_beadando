using Autoberles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ModositBerles.xaml
    /// </summary>
    public partial class ModositBerles : Window
    {
        private Database db;
        public Berles? berles;

        public ModositBerles(Database db, Berles? berles)
        {
            InitializeComponent();

            this.Title = berles == null ? "Adat hozzáadás" : $"Módosítás (Bérlés #{berles?.id})";
            this.berles = berles;
            this.db = db;

            submitBtn.Content = berles == null ? "Hozzáadás" : "Módosítás";

            auto.ItemsSource = db.cn.Autok.ToList().Select(a => $"#{a.id} {a.marka} {a.tipus}").ToList();

            if (berles != null)
            {
                berlo.Text = berles.berlo;
                kezdo_datum.Text = berles.kezdo_datum.ToString();
                vege_datum.Text = berles.vege_datum.ToString();
                auto.SelectedItem = $"#{berles.Auto.id} {berles.Auto.marka} {berles.Auto.tipus}";
            }
        }

        private void modositClick(object sender, RoutedEventArgs e)
        {
            if (berlo.Text == "")
            {
                MessageBox.Show("A bérlő neve nem lehet üres!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (kezdo_datum.Text == "")
            {
                MessageBox.Show("A kezdő dátum nem lehet üres!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (vege_datum.Text == "")
            {
                MessageBox.Show("A vége dátum nem lehet üres!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DateTime kezdo = DateTime.Parse(kezdo_datum.Text);
            DateTime vege = DateTime.Parse(vege_datum.Text);
            if (kezdo > vege)
            {
                MessageBox.Show("A kezdő dátum nem lehet később mint a vége dátum!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (auto.SelectedItem == null)
            {
                MessageBox.Show("A bérlés hozzáadásához ki kell választani egy autót!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            short id = this.berles != null ? this.berles.id : (short)(-1);
            short autoId = short.Parse(auto.SelectedItem.ToString()!.Split(' ')[0].Split('#')[1]);

            this.berles = new Berles()
            {
                id = id,
                berlo = berlo.Text,
                kezdo_datum = kezdo,
                vege_datum = vege,
                Auto = db.cn.Autok.First(a => a.id == autoId)
            };

            this.Close();
        }

        private void megseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
