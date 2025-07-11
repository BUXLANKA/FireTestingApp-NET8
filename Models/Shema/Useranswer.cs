using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FireTestingApp_net8.Models.Shema;

[Table("useranswers", Schema = "fire_safety_system")]
public partial class Useranswer
{
    [Key]
    [Column("useranswerid")]
    public int Useranswerid { get; set; }

    [Column("answerdate", TypeName = "timestamp without time zone")]
    public DateTime Answerdate { get; set; }

    [Column("userid")]
    public int? Userid { get; set; }

    [Column("questionid")]
    public int? Questionid { get; set; }

    [Column("answerid")]
    public int? Answerid { get; set; }

    [Column("iscorrect")]
    public bool Iscorrect { get; set; }

    [ForeignKey("Answerid")]
    [InverseProperty("Useranswers")]
    public virtual Answer? Answer { get; set; }

    [ForeignKey("Questionid")]
    [InverseProperty("Useranswers")]
    public virtual Question? Question { get; set; }

    [ForeignKey("Userid")]
    [InverseProperty("Useranswers")]
    public virtual User? User { get; set; }
}
