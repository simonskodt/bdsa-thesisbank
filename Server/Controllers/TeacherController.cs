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

    [Authorize(Roles = "Teacher")]
    [HttpGet("{teahcerID}")]
    public async Task<IReadOnlyCollection<ApplyDTOWithMinalThesis>> Get(int teahcerID)
    => await _teacher_repository.ReadPendingApplicationsByTeacherID(teahcerID);
    
    [Authorize(Roles = "Teacher")]
    [AllowAnonymous]
    [HttpPut("{applyid}")]
    public async Task<ApplyDTO> Update(int applyid, [FromBody] ApplyDTOids dto)
        => (await _teacher_repository.Accept(dto.studentID, dto.thesisID)).Item2;

}