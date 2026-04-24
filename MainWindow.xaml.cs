using Autoberles;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using vizprog_beadando.db;

namespace vizprog_beadando
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Database db = new();

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Autóbérlés - Autók";
            dgAutok.Visibility = Visibility.Visible;
            updateData();
        }
       
        private void updateData()
        {
            if (dgAutok.Visibility == Visibility.Visible)
            {
                dgAutok.ItemsSource = db.cn.Autok.ToList().FindAll(i => i.marka.ToLower().Contains(kereses.Text.ToLower()));
            }
            else
            {
                dgBerlesek.ItemsSource = db.cn.Berlesek.Include(p => p.Auto).ToList().FindAll(i => i.berlo.ToLower().Contains(kereses.Text.ToLower()));
            }
        }

        private void menuAutokClick(object sender, RoutedEventArgs e)
        {
            this.Title = "Autóbérlés - Autók";
            dgBerlesek.Visibility = Visibility.Collapsed;
            dgAutok.Visibility = Visibility.Visible;
            kereses.Text = "";
            updateData();
        }

        private void menuBerlesekClick(object sender, RoutedEventArgs e)
        {
            this.Title = "Autóbérlés - Bérlések";
            dgAutok.Visibility = Visibility.Collapsed;
            dgBerlesek.Visibility = Visibility.Visible;
            kereses.Text = "";
            updateData();
        }

        private void menuUj(object sender, RoutedEventArgs e)
        {

        }

        private void menuModosit(object sender, RoutedEventArgs e)
        {
            DataGrid dg = dgAutok.Visibility == Visibility.Visible ? dgAutok : dgBerlesek;
            if (dg.SelectedItems.Count == 0)
            {
                MessageBox.Show("Nincs kijelölt elem!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (int i = 0; i < dg.SelectedItems.Count; i++)
            {
                if (dg.SelectedItems[i] is Auto auto)
                {
                    ModositAuto ma = new ModositAuto(auto);
                    ma.ShowDialog();
                }
                else if (dg.SelectedItems[i] is Berles berles)
                {
                    ModositBerles mb = new ModositBerles(berles);
                    mb.ShowDialog();
                }
            }

            updateData();
        }

        private void menuTorles(object sender, RoutedEventArgs e)
        {
            DataGrid dg = dgAutok.Visibility == Visibility.Visible ? dgAutok : dgBerlesek;
            if (dg.SelectedItems.Count == 0)
            {
                MessageBox.Show("Nincs kijelölt elem!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Törölni szeretné a kijelölt {(dg.SelectedItems.Count == 1 ? "elemet" : "elemeket")}?", "Törlés", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in dg.SelectedItems)
                {
                    if (dg == dgAutok)
                        db.cn.Autok.Remove(db.cn.Autok.Single(a => a.id == ((Auto)item).id));
                    else
                        db.cn.Berlesek.Remove(db.cn.Berlesek.Single(b => b.id == ((Berles)item).id));
                }
            }

            db.cn.SaveChanges();
            updateData();
        }

        private void keresesChange(object sender, TextChangedEventArgs e)
        {
            updateData();
        }
    }
}