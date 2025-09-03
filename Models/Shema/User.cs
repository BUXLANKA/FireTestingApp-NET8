using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireTestingApp_net8.Models.Shema;

[Table("users", Schema = "fire_safety_system")]
[Index("Userlogin", Name = "uq__users__7f8e8d5e5089521a", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("userid")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Userid { get; set; }

    [Column("firstname")]
    [StringLength(50)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(50)]
    public string Lastname { get; set; } = null!;

    [Column("surname")]
    [StringLength(50)]
    public string? Surname { get; set; }

    [Column("roleid")]
    public int Roleid { get; set; }

    [Column("userlogin")]
    [StringLength(50)]
    public string Userlogin { get; set; } = null!;

    [Column("userpassword")]
    [StringLength(12)]
    public string Userpassword { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    [ForeignKey("Roleid")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("Fromuser")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    [InverseProperty("User")]
    public virtual ICollection<Useranswer> Useranswers { get; set; } = new List<Useranswer>();
}
