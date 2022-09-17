using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_proj_ostateczny
{
    /// <summary>
    /// Abstrakcyjna klasa zapewniająca pole id dla każdej (poza przystanki_na_trasach) z klas reprezentujących tabele w bazie danych.
    /// </summary>
    public abstract class IdClass
    {
        public abstract int id { get; set; }
    }
}
