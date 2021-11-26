namespace Entities;

public class Thesis
{
    public int Id {get; set;}

    [StringLength(20)]
    public string? Name {get; set;}

    public string? Description {get; set;}

    public Teacher Teacher {get; set;}

    public ICollection<Apply>? Applies { get; set; } 
    
}