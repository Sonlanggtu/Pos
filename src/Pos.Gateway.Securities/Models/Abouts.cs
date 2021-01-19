using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pos.Gateway.Securities.Models
{
    public partial class Abouts
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
    }
}
