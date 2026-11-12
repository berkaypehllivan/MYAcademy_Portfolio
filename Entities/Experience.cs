namespace MYAcademy_Portfolio.Entities;

public class Experience : IOrderable
{
    public int ExperienceId { get; set; }
    public string Title { get; set; }
    public string CompanyName { get; set; }
    public string Description { get; set; }
    public string SideTitle { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string WorkType { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}
