using System.Collections.Generic;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;


namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentRepository _repository;

    public StudentController(IStudentRepository repository) {
         _repository = repository;
    }

    // [Authorize]
    // [HttpPost]
    // [ProducesResponseType(typeof(bool), 201)]
    // public async ThesisBank<IActionResult> Post(int studentId, int thesisId, Status status) {
    //     // Must update Applied-table with studentId, thesisId and status
    // }

    [AllowAnonymous]
    [HttpGet("{name}")]
    public async Task<int?> Get(string name){
        var item = await _repository.ReadStudentIDByName(name);
        Console.WriteLine("HALLOOOO::::: " + item.Item2);
        return item.Item2;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ApplyDTO>> Post(ApplyDTO applyDTO){
        var created = await _repository.ApplyForThesis(applyDTO.Student.Id, applyDTO.Thesis.Id);
        Console.WriteLine("ITEM2 ::: " + created.Item2);
        Console.WriteLine("Student ::: " + applyDTO.Student.Id);
        Console.WriteLine("Thesis ::: " + applyDTO.Thesis.Id);



        return CreatedAtAction(nameof(Get),
            new {id = created}, created);

        //return new CreatedResult(created);
    }

    

}