using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireTestingApp_net8.Models.Shema;

[Table("questions", Schema = "fire_safety_system")]
public partial class Question
{
    [Key]
    [Column("questionid")]
    public int Questionid { get; set; }

    [Column("questiontext")]
    public string Questiontext { get; set; } = null!;

    [InverseProperty("Question")]
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    [InverseProperty("Question")]
    public virtual ICollection<Useranswer> Useranswers { get; set; } = new List<Useranswer>();
}
