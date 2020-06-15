using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IzinUygulamasi.Models.Custom
{
    using System;
    using System.Collections.Generic;
    public partial class OnayIzinler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public int izin_id { get; set; }
        public DateTime baslangic_tarihi { get; set; }
        public DateTime bitis_tarihi { get; set; }
        public string izin_turu { get; set; }
        public string izin_aciklamasi { get; set; }
        public string adi_soyadi { get; set; }

    }
}