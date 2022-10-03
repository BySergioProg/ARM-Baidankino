namespace ARM_Baidankino.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserData")]
    public partial class UserData
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

       // [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string UserName { get; set; }

      //  [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Password { get; set; }

       // [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccessLevel { get; set; }
    }
}
