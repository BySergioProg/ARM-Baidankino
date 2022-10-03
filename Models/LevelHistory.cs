namespace ARM_Baidankino.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LevelHistory")]
    public partial class LevelHistory
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }

        [Key]
        [Column(Order = 3)]
        public float E1Level { get; set; }

        [Key]
        [Column(Order = 4)]
        public float E2Level { get; set; }
    }
}
