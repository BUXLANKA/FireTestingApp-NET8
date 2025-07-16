using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireTestingApp_net8.Models.Shema;

[Table("answers", Schema = "fire_safety_system")]
public partial class Answer
{
    [Key]
    [Column("answerid")]
    public int Answerid { get; set; }

    [Column("questionid")]
    public int Questionid { get; set; }

    [Column("answertext")]
    public string Answertext { get; set; } = null!;

    [Column("iscorrectanswer")]
    public bool Iscorrectanswer { get; set; }

    [ForeignKey("Questionid")]
    [InverseProperty("Answers")]
    public virtual Question Question { get; set; } = null!;

    [InverseProperty("Answer")]
    public virtual ICollection<Useranswer> Useranswers { get; set; } = new List<Useranswer>();
}
