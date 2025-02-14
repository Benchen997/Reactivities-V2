namespace Application.Activities.DTOs;

public class BaseActivityDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string City { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}