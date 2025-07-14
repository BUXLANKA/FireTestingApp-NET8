using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FireTestingApp_net8.Models.Shema;

[Table("tickets", Schema = "fire_safety_system")]
public partial class Ticket
{
    [Key]
    [Column("ticketid")]
    public int Ticketid { get; set; }

    [Column("fromuserid")]
    public int Fromuserid { get; set; }

    [Column("ticketdate", TypeName = "timestamp without time zone")]
    public DateTime Ticketdate { get; set; }

    [Column("tickettext", TypeName = "character varying")]
    public string Tickettext { get; set; } = null!;

    [ForeignKey("Fromuserid")]
    [InverseProperty("Tickets")]
    public virtual User Fromuser { get; set; } = null!;
}
