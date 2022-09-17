using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace cs_proj_ostateczny
{
    /// <summary>
    /// Klasa zawierająca funkcje przydatne w pozostałych plikach.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Funkcja zwraca następne id w danej tabeli bazy danych.
        /// </summary>
        /// <param name="db">Tabela bazy danych w której szukane jest id.</param>
        /// <returns>Następne wolne id.</returns>
        public static int getNextId(DbSet db)
        {
            var current_id = 0;
            foreach (var entity in db)
            {
                IdClass e = (IdClass)entity;

                if (e.id > current_id)
                {
                    current_id = e.id;
                }
            }

            return current_id + 1;
        }
    }
}
