//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cs_proj_ostateczny
{
    using System;
    using System.Collections.Generic;
    
    public partial class Przystanki_na_trasie
    {
        public int id_trasy { get; set; }
        public int id_przystanku { get; set; }
        public int numer_na_trasie { get; set; }
    
        public virtual Przystanki Przystanki { get; set; }
        public virtual Trasy Trasy { get; set; }
    }
}
