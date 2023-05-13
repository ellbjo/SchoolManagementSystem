using SchoolManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
namespace SchoolManagementSystem.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TeacherId { get; set; }
    public int ClassId { get; set; }

    [ForeignKey("TeacherId")]
    public Teacher Teacher { get; set; }

    [ForeignKey("ClassId")]
    public Class Class { get; set; }
}
