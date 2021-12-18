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
        return (await _repository.ReadStudentIDByName(name)).Item2;
    }

    //[Authorize(Roles = "Teacher")]
    [AllowAnonymous]
    [HttpPut("{applyid}")]
    public async Task<ApplyDTO> Update (int applyid, [FromBody] ApplyDTOid dto){
/*         if (dto.Status == ??){
            var respone = await _teacher_repository.Accept(dto.studentID, dto.thesisID);
            return respone.Item2;

        }else{
            var respone = await _teacher_repository.Reject(dto.studentID, dto.thesisID);
            return respone.Item2;

        } */
        Console.WriteLine("UPDATECONTROLLER before Accept --  " );
        var respone = await _repository.Accept(dto.studentID, dto.thesisID);
        Console.WriteLine("UPDATECONTROLLER after Accept --  " );
        return respone.Item2;
    }

    [HttpDelete("{studentID}")]
    public async Task<Response> Delete(int studentID)
        => (await _repository.RemoveAllPendings(studentID));

    // [AllowAnonymous]
    // [HttpPost]
    // public async Task<ActionResult<ApplyDTO>> Post(ApplyDTO applyDTO){
    //     var created = await _repository.ApplyForThesis(applyDTO.Student.Id, applyDTO.Thesis.Id);
    //     Console.WriteLine("ITEM2 ::: " + created.Item2);
    //     Console.WriteLine("Student ::: " + applyDTO.Student.Id);    
    //     Console.WriteLine("Thesis ::: " + applyDTO.Thesis.Id);



    //     return CreatedAtAction(nameof(Get),
    //         new {id = created}, created);

        //return new CreatedResult(created);
    //}

    

}