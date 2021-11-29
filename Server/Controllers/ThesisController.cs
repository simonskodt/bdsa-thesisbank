using System.Collections.Generic;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThesisBank.Server.Data;

namespace ThesisBank.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ThesesController : ControllerBase {

    private readonly DataContext _context;

    public ThesesController(DataContext context) {
        _context = context;
    }

    // [HttpGet]
    // public ActionResult<List<Thesis>> GetAllThesesPosts() {
    //     Teacher Rasmus = new Teacher{Id = 2, Name = "Rasmus", Email = "Rasmus@itu.dk"};
        
    //     // return Ok(_context.ThesesPosts);
    //     return new List<Thesis> { 
    //         new Thesis { Id = 1, Name = "Linq", Teacher = Rasmus }
    //      };
    // }

    [HttpGet]
    public IEnumerable<Thesis> GetAllThesesPosts() {        
        
        Teacher Rasmus = new Teacher{Id = 2, Name = "Rasmus", Email = "Rasmus@itu.dk"};

        return Enumerable.Range(5).Select(thesis => new Thesis {
            Id = 1, Name = "Linq", Teacher = Rasmus
        }).ToArray();
    }
    
}