namespace Entities;

public class Thesis
{
    public int Id {get; set;}

    [StringLength(20)]
    public string? name {get; set;}

    public string? description {get; set;}

    public Teacher teacher {get; set;}

    public ICollection<Apply>? applies { get; set; } 
    
}