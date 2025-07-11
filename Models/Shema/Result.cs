using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FireTestingApp_net8.Models.Shema;

[Table("results", Schema = "fire_safety_system")]
public partial class Result
{
    [Key]
    [Column("resultid")]
    public int Resultid { get; set; }

    [Column("userid")]
    public int Userid { get; set; }

    [Column("testdate", TypeName = "timestamp without time zone")]
    public DateTime Testdate { get; set; }

    [Column("userscore")]
    public int Userscore { get; set; }

    [Column("statusid")]
    public int Statusid { get; set; }

    [ForeignKey("Statusid")]
    [InverseProperty("Results")]
    public virtual Teststatus Status { get; set; } = null!;

    [ForeignKey("Userid")]
    [InverseProperty("Results")]
    public virtual User User { get; set; } = null!;
}
