using Autoberles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

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
            int db = 0;
            try
            {
                StreamReader r = new StreamReader("db/autok.csv");
                while (!r.EndOfStream)
                {
                    string[] data = r.ReadLine()!.Split(',');
                    Auto row = new Auto
                    {
                        marka = data[0],
                        tipus = data[1],
                        berles_dij = Convert.ToInt32(data[2])
                    };

                    cn.Autok.Add(row);
                    db++;
                }

                MessageBox.Show($"{db} példa adat feltöltése sikeresen megtörtént!", "Automatikus adatfeltöltés");
                r.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a fájlok betöltése közben!\n{ex.Message}");
            }
        }
    }
}
