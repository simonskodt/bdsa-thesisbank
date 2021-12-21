using System.Collections.Generic;
namespace Server.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ThesesController : ControllerBase
{
    readonly IThesisRepository _repository;

    public ThesesController(IThesisRepository repository)
    {
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IReadOnlyCollection<ThesisDTOMinimal>> Get(int id)
        => await _repository.ReadNonPendingTheses(id);
}