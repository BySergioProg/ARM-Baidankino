namespace ARM_Baidankino.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Alarm
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public DateTime? DateAdopt { get; set; }

        [StringLength(50)]
        public string AdoptUser { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string AlarmType { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string AlarmText { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string Device { get; set; }
    }
}
