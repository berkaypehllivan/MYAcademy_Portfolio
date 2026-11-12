namespace MYAcademy_Portfolio.Entities;

public class Project
{
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string GithubUrl { get; set; }
    public string CoverImageUrl { get; set; }
    public string? Keywords { get; set; }
    public string? LiveDemoUrl { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public bool IsActive { get; set; }

}
