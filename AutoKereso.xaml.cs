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
    /// Interaction logic for AutoKereso.xaml
    /// </summary>
    public partial class AutoKereso : Window
    {
        public string? autoFilter;

        public AutoKereso()
        {
            InitializeComponent();
            autoKereses.Focus();
        }

        private void autoKereso_Click(object sender, RoutedEventArgs e)
        {
            this.autoFilter = autoKereses.Text.ToLower();
            this.Close();
        }
    }
}
