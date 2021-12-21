namespace Server.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    readonly IStudentRepository _repository;

    public StudentController(IStudentRepository repository)
    {
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet("{name}")]
    public async Task<int?> Get(string name)
    {
        return (await _repository.ReadStudentIDByName(name)).Item2;
    }

    [Authorize(Roles = "Student")]
    [HttpPut("{applyid}")]
    public async Task<ApplyDTO> Update(int applyid, [FromBody] ApplyDTOids dto)
        => (await _repository.Accept(dto.studentID, dto.thesisID)).Item2;


    [Authorize(Roles = "Student")]
    [HttpDelete("{studentID}")]
    public async Task<Response> Delete(int studentID)
        => (await _repository.RemoveAllApplications(studentID));
}