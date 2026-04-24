using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Autoberles;

namespace vizprog_beadando.db
{
    internal class Database
    {
        public cnAutoberles cn { get; set; }

        public Database()
        {
            cn = new cnAutoberles();

            cn.Database.EnsureCreated();
            if (cn.Autok == null) return;

            if (!cn.Autok.Any())
            {
                FillWithData();
            }
        }

        private void FillWithData()
        {
            
        }
    }
}
