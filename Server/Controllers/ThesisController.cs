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
public class ThesisController : ControllerBase {
    private readonly IThesisRepository _repository;

    public ThesisController(IThesisRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<MinimalThesisDTO>> Get()
        => await _repository.ReadAll();
    
}
