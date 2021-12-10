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


    [HttpGet("{studentID}")]
    public async Task<IReadOnlyCollection<ApplyDTO>> Get(int studentID, Status status)
    => await _repository.ReadAppliedByStudentAndStatus(studentID, Status.Accepted);

/*     [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<ApplyDTOid>> Get()
        => await _repository.ReadApply(); */

/*     [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(ApplyDTOid), 201)]
    public async Task<IActionResult> Post(ApplyDTO applyDTO)
    {
        
        // var created = await _repository.CreateAsync(applyDTO.Student.Id, applyDTO.Thesis.Id);

        // return CreatedAtAction(nameof(Get), new { created.Thesis.Id }, created);

        Console.WriteLine("Student ::: " + applyDTO.Student.Id);
        Console.WriteLine("Thesis ::: " + applyDTO.Thesis.Id);
        var created = await _repository.ApplyForThesis(applyDTO.Student.Id, applyDTO.Thesis.Id);
        Console.WriteLine("ITEM2 ::: " + created.Item2.studentID);
        Console.WriteLine("Hello world");



        return CreatedAtAction(nameof(Get), new {id = 1}, created.Item2); */


        // return new CreatedResult(created);
    
}