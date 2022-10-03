namespace ARM_Baidankino.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OilCount")]
    public partial class OilCount
    {
        public int id { get; set; }

        public DateTime Date { get; set; }

        [Column("OilCount")]
        public float OilCount1 { get; set; }
    }
}
