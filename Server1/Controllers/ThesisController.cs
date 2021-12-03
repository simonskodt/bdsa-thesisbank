using System.Collections.Generic;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Entities;

namespace Server.Controllers;

// [Authorize]
// [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class ThesesController : ControllerBase {
    public List<Thesis> Theses { get; set; } = new List<Thesis> {
        new Thesis ("WildAlgorithms") { Id = 1, Teacher = new Teacher ("Rasmus") {
            Id = 1, Email = "Rasmus@itu.dk"
        } },
        new Thesis ("GraphAlgorithms") { Id = 2, Teacher = new Teacher ("Rasmus") {
            Id = 1, Email = "Rasmus@itu.dk"
        } },
        new Thesis ("Linq") { Id = 2, Teacher = new Teacher ("Rasmus") {
            Id = 1, Email = "Rasmus@itu.dk"
        } },
    };

    [HttpGet]
    public ActionResult<List<Thesis>> Get() {
        return Ok(Theses);
    }
}