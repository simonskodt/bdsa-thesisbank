using System.Collections.Generic;
namespace Server.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ThesisController : ControllerBase
{
    readonly IThesisRepository _repository;

    public ThesisController(IThesisRepository repository)
    {
        _repository = repository;
    }

    [Authorize(Roles = "Student")]
    [HttpGet("{id}")]
    public async Task<ThesisDTO> Get(int id)
        => (await _repository.ReadThesis(id)).Item2;

    [AllowAnonymous]
    [HttpGet("{StudentID}/{ThesisID}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ApplyDTOids), 201)]
    public async Task<ActionResult<ApplyDTOids>> Get(int StudentID, int ThesisID)
    {
        var repsonse = await _repository.FindApplyDTOid(StudentID, ThesisID);

        if (repsonse.Item1 == Core.Response.Success)
        {
            return repsonse.Item2;
        }
        else
        {
            return new NotFoundResult();
        }
    }
}