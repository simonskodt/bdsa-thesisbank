namespace Core;

public record StudentDTO(int Id, string Name, string Email);

public record StudentIDDTO(int id);

// public record StudentDTO
// {
//     public int Id { get; init; }
//     public string Name { get; init; }
//     public string Email { get; init; }
// }