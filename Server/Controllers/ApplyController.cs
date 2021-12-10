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

    //[Authorize(Roles = "Student")]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<ApplyDTOid>> Get()
        => await _repository.ReadApplied();

    [AllowAnonymous]
    [HttpGet("{studentID}")]
    public async Task<IReadOnlyCollection<ApplyDTO>> Get(int studentID)
    => await _repository.ReadAppliedByStudentAndStatus(studentID);

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(ApplyDTOid), 201)]
    public async Task<IActionResult> Post(ApplyDTO applyDTO)
    {
        
        // var created = await _repository.CreateAsync(applyDTO.Student.Id, applyDTO.Thesis.Id);

        // return CreatedAtAction(nameof(Get), new { created.Thesis.Id }, created);

        Console.WriteLine("Student ::: " + applyDTO.Student.Id);
        Console.WriteLine("Thesis ::: " + applyDTO.Thesis.Id);
        var created = await _repository.ApplyForThesis(applyDTO.Student.Id, applyDTO.Thesis.Id);
        Console.WriteLine("ITEM2 ::: " + created.Item2);
        Console.WriteLine("Hello world");

        return CreatedAtAction(nameof(Get), new {Id = created.Item2.Id}, created.Item2); 


        // return new CreatedResult(created);
    
}
}