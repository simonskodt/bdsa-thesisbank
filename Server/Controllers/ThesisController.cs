using System.Collections.Generic;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Core;

namespace ThesisBank.Server.Controllers;

[Authorize]
// [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class ThesesController : ControllerBase {

    private readonly IThesisRepository _repository;

    public ThesesController(IThesisRepository repository)
    {
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<MinimalThesisDTO>> Get()
        => await _repository.ReadAll();
}