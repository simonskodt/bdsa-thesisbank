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

    [HttpGet]
    public async Task<IReadOnlyCollection<MinimalThesisDTO>> Get()
        => await _repository.ReadAll();


    [AllowAnonymous]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ThesisDTO), 200)]
    [HttpGet("{id}")]
    public async Task<ThesisDTO> Get(int id)
        => (await _repository.ReadThesis(id)).Item2;

    // [Authorize]
    // [HttpPost]
    // [ProducesResponseType(typeof(bool), 201)]
    // public async Task<IActionResult> Post(int studentId, int thesisId, bool isApplied) {
    //     var applied = await _repository.CreateAsync()
    // }

}