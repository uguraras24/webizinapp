//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IzinUygulamasi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class izinler
    {
        public int izin_id { get; set; }
        public Nullable<int> personel_id { get; set; }
        public Nullable<int> alt_yonetici_id { get; set; }
        public Nullable<System.DateTime> olusturulma_tarihi { get; set; }
        public Nullable<System.DateTime> baslangic_tarihi { get; set; }
        public Nullable<System.DateTime> bitis_tarihi { get; set; }
        public Nullable<int> izin_turu { get; set; }
        public string izin_aciklamasi { get; set; }
        public Nullable<int> izin_durumu { get; set; }
    
        public virtual izinTurleri izinTurleri { get; set; }
        public virtual personeller personeller { get; set; }
    }
}
