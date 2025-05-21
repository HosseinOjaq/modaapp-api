namespace ModaApp.Domain.Entities;

public class PageVisit
{
    public int Id { get; set; }
    public string? IPAddress { get; set; }
    public string? PageUrl { get; set; }
    public DateTime VisitDate { get; set; }
}