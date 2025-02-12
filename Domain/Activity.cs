namespace Domain;

public class Activity
{
    // string is easier to work with than Guid
    
    public string Id { get; init; } = Guid.NewGuid().ToString();
    
    public required string Title { get; set; }
    
    public DateTime Date { get; set; }
    
    public required string Description { get; set; }
    
    public required string Category { get; set; }
    
    public required string City { get; set; }
    
    public required string Venue { get; set; }
    
    public bool IsCancelled { get; set; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}