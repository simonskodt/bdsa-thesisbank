using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;
namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]public class ApplyController : ControllerBase
{
    private readonly IApplyRepository _repository;

    public ApplyController(IApplyRepository repository) {
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<ApplyDTOids>> Get()
        => await _repository.ReadApplied();

    [Authorize(Roles = "Student")]
    [HttpGet("{studentID}")]
    public async Task<IReadOnlyCollection<ApplyDTOWithMinalThesis>> Get(int studentID)
    => await _repository.ReadAppliedByStudentID(studentID);

    [AllowAnonymous]
    [HttpDelete("{Applyid}")]
    public async Task<Response> Delete(int applyID)
        => (await _repository.DeleteApplied(applyID));

    [Authorize(Roles = "Student")]
    [HttpPost]
    [ProducesResponseType(typeof(ApplyPostDTO), 201)]
    public async Task<IActionResult> Post(ApplyPostDTO applyDTO)
    {
        var created = await _repository.ApplyForThesis(applyDTO.studentID, applyDTO.Thesis.Id);
        return CreatedAtAction(nameof(Get), new {Id = created.Item2.Id}, created.Item2); 
    }
}