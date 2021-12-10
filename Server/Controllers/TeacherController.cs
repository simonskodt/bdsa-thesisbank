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
    private readonly IApplyRepository _apply_repository;


    public TeacherController(ITeacherRepository teacherRepository, IApplyRepository applyRepository) {
          _teacher_repository = teacherRepository;
          _apply_repository = applyRepository;
    }

    [Authorize(Roles = "Student")]
    [HttpGet("{teahcerID}")]
    public async Task<IReadOnlyCollection<ApplyDTO>> Get(int teahcerID)
    => await _apply_repository.ReadApplicationsByTeacherID(teahcerID);


}