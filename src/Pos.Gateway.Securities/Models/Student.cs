using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Gateway.Securities.Models
{
    public partial class Student
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(25)]
        public string Name { get; set; }
        public int? Old { get; set; }
    }
}
