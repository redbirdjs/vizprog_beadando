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
            keresesTorles.IsEnabled = kereses.Text.Length != 0;
            updateData();
        }
       
        private void updateData(string? filter = null)
        {
            if (dgAutok.Visibility == Visibility.Visible)
            {
                dgAutok.ItemsSource = db.cn.Autok.ToList().FindAll(i => filter == null ? i.marka.ToLower().Contains(kereses.Text.ToLower()) : i.marka.ToLower().Contains(filter));
            }
            else
            {
                dgBerlesek.ItemsSource = db.cn.Berlesek.Include(p => p.Auto).ToList().FindAll(i => filter == null ? i.berlo.ToLower().Contains(kereses.Text.ToLower()) : i.berlo.ToLower().Contains(filter));
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
            if (dgAutok.Visibility == Visibility.Visible)
            {
                ModositAuto ma = new(null);
                ma.ShowDialog();

                if (ma.auto == null) return;

                db.cn.Autok.Add(new Auto {
                    marka = ma.auto.marka,
                    tipus = ma.auto.tipus,
                    berles_dij = ma.auto.berles_dij
                });
            }
            else
            {
                ModositBerles mb = new(db, null);
                mb.ShowDialog();

                if (mb.berles == null) return;

                db.cn.Berlesek.Add(new Berles
                {
                    berlo = mb.berles.berlo,
                    kezdo_datum = mb.berles.kezdo_datum,
                    vege_datum = mb.berles.vege_datum,
                    Auto = db.cn.Autok.First(a => a.id == mb.berles.Auto.id)
                });
            }

            db.cn.SaveChanges();
            updateData();
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
                if (dg == dgAutok)
                {
                    ModositAuto ma = new((Auto)dg.SelectedItems[i]!);
                    ma.ShowDialog();

                    if (ma.auto == null) continue;

                    Auto a = db.cn.Autok.First(a => a.id == ma.auto.id);
                    a.marka = ma.auto.marka;
                    a.tipus = ma.auto.tipus;
                    a.berles_dij = ma.auto.berles_dij;
                }
                else
                {
                    ModositBerles mb = new(db, (Berles)dg.SelectedItems[i]!);
                    mb.ShowDialog();

                    if (mb.berles == null) continue;

                    Berles b = db.cn.Berlesek.First(b => b.id == mb.berles.id);
                    b.berlo = mb.berles.berlo;
                    b.kezdo_datum = mb.berles.kezdo_datum;
                    b.vege_datum = mb.berles.vege_datum;
                    b.Auto = db.cn.Autok.First(a => a.id == mb.berles.Auto.id);
                }
            }

            db.cn.SaveChanges();
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
            keresesTorles.IsEnabled = kereses.Text.Length != 0;
            updateData();
        }

        private void menuAutoKereso(object sender, RoutedEventArgs e)
        {
            this.Title = "Autóbérlés - Autók";
            dgBerlesek.Visibility = Visibility.Collapsed;
            dgAutok.Visibility = Visibility.Visible;
            kereses.Text = "";
            updateData();

            AutoKereso ak = new AutoKereso();
            ak.ShowDialog();

            if (ak.autoFilter == null) return;

            kereses.Text = ak.autoFilter;
            updateData(ak.autoFilter.ToLower());
        }

        private void menuBerlesKereso(object sender, RoutedEventArgs e)
        {
            this.Title = "Autóbérlés - Bérlések";
            dgAutok.Visibility = Visibility.Collapsed;
            dgBerlesek.Visibility = Visibility.Visible;
            kereses.Text = "";
            updateData();

            BerloKereso bk = new BerloKereso();
            bk.ShowDialog();

            if (bk.berloFilter == null) return;

            kereses.Text = bk.berloFilter;
            updateData(bk.berloFilter.ToLower());
        }

        private void KeresesTorles_Click(object sender, RoutedEventArgs e)
        {
            keresesTorles.IsEnabled = false;
            kereses.Text = "";
        }
    }
}