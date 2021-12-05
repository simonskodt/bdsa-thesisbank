namespace Entities;

public class Thesis
{
    public int Id {get; set;}

    [StringLength(60)]
    public string? Name {get; set;}

    public string? Description {get; set;}

    // // todo: Keywords
    // public string[]? Keywords { get; set; }

    // // TODO: Excerpt
    // public string? excerpt { get; set; }

    public Teacher Teacher {get; set;}

    public ICollection<Apply>? Applies { get; set; } 

     public Thesis(string name){
        Name = name;
    }
    
}