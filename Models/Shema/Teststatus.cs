using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FireTestingApp_net8.Models.Shema;

[Table("teststatuses", Schema = "fire_safety_system")]
public partial class Teststatus
{
    [Key]
    [Column("statusid")]
    public int Statusid { get; set; }

    [Column("statusname")]
    [StringLength(10)]
    public string Statusname { get; set; } = null!;

    [InverseProperty("Status")]
    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
