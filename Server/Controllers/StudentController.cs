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
    public async Task<(Response, int?)> Get(string name){
        var item = await _repository.ReadStudentIDByName(name);
        Console.WriteLine("HALLOOOO::::: " + item.Item2);
        return item;
    }
}