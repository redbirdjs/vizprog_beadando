using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace vizprog_beadando
{
    /// <summary>
    /// Interaction logic for BerloKereso.xaml
    /// </summary>
    public partial class BerloKereso : Window
    {
        public string? berloFilter;

        public BerloKereso()
        {
            InitializeComponent();
            berloKereses.Focus();
        }

        private void berloKereses_Click(object sender, RoutedEventArgs e)
        {
            this.berloFilter = berloKereses.Text;
            this.Close();
        }
    }
}
