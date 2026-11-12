namespace MYAcademy_Portfolio.Entities;

public class Skill : IOrderable
{
    public int SkillId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Keywords { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}
