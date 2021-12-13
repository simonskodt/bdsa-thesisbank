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
public class ThesesController : ControllerBase
{
    private readonly IThesisRepository _repository;

    public ThesesController(IThesisRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public async Task<IReadOnlyCollection<MinimalThesisDTO>> Get(int id)
        => await _repository.ReadNonPendingTheses(id);
}