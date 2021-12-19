using System.Collections.Generic;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;


namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{
    private readonly ITeacherRepository _teacher_repository;


    public TeacherController(ITeacherRepository teacherRepository, IApplyRepository applyRepository) {
          _teacher_repository = teacherRepository;
    }
    
    // [AllowAnonymous]
    // [HttpGet("{name}")]
    // public async Task<int?> Get(string name){
    //     return (await _teacher_repository.ReadTeacherIDByName(name)).Item2;
    // }

    //[Authorize(Roles = "Teacher")]

    [HttpGet("{teahcerID}")]
    public async Task<IReadOnlyCollection<ApplyWithIDDTO>> Get(int teahcerID)
    => await _teacher_repository.ReadPendingStudentApplication(teahcerID);
    
    //[Authorize(Roles = "Teacher")]
    [AllowAnonymous]
    [HttpPut("{applyid}")]
    public async Task<ApplyDTO> Update(int applyid, [FromBody] ApplyDTOid dto){
/*         if (dto.Status == ??){
            var respone = await _teacher_repository.Accept(dto.studentID, dto.thesisID);
            return respone.Item2;

        }else{
            var respone = await _teacher_repository.Reject(dto.studentID, dto.thesisID);
            return respone.Item2;

        } */
        var respone = await _teacher_repository.Accept(dto.studentID, dto.thesisID);
        return respone.Item2;
    }

}