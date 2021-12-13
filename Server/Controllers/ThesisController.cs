using System.Collections.Generic;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;

namespace Server.Controllers;


[Authorize]
[ApiController]
// [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[Route("api/[controller]")]
public class ThesisController : ControllerBase
{
    private readonly IThesisRepository _repository;

    public ThesisController(IThesisRepository repository)
    {
        _repository = repository;
    }


  /*   [AllowAnonymous]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ThesisDTO), 200)]
    [HttpGet("{id}")] */
/*     public async Task<ThesisDTO> Get(int id)
        => await _repository.ReadThesis(id)
 */
    //[Authorize(Roles = "Student")]
    [HttpGet("{id}")]
    public async Task<ThesisDTO> Get(int id)
        => (await _repository.ReadThesis(id)).Item2;

    [HttpGet("{StudentID}/{ThesisID}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ApplyDTOid), 201)]

    public async Task<ActionResult<ApplyDTOid>> Get(int StudentID, int ThesisID){

        var repsonse = await _repository.FindApplyDTOID(StudentID, ThesisID);

        if(repsonse.Item1 == Core.Response.Success){
            return repsonse.Item2;
        }else{
            return new NotFoundResult();
        }
    }



    // [Authorize]
    // [HttpPost]
    // [ProducesResponseType(typeof(bool), 201)]
    // public async Task<IActionResult> Post(int studentId, int thesisId, bool isApplied) {
    //     var applied = await _repository.CreateAsync()
    // }

}