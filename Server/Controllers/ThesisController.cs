using System.Collections.Generic;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data;

namespace Server.Controllers;

// [Authorize]
// [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[ApiController]
[Route("api/[controller]")]
public class ThesesController : ControllerBase {
    public List<Thesis> Theses { get; set; } = new List<Thesis> {
        new Thesis {
            new Thesis { Id = 1, Name = "WildAlgorithms", Teacher = new Teacher {
                Id = 1, Name = "Rasmus", Email = "Rasmus@itu.dk"
            } },
            new Thesis { Id = 2, Name = "GraphAlgorithms", Teacher = new Teacher {
                Id = 1, Name = "Rasmus", Email = "Rasmus@itu.dk"
            } },
            new Thesis { Id = 2, Name = "Linq", Teacher = new Teacher {
                Id = 1, Name = "Rasmus", Email = "Rasmus@itu.dk"
            } },
        }
    };

    [HttpGet]
    public ActionResult<List<Thesis>> Get() {
        return Ok(Theses);
    }
}