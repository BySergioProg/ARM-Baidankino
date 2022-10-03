namespace ARM_Baidankino.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ManualOil")]
    public partial class ManualOil
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

       // [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string UserName { get; set; }

       // [Key]
        [Column(Order = 2)]
        public DateTime Date { get; set; }

       // [Key]
        [Column(Order = 3)]
        public float OilCount { get; set; }
    }
}
