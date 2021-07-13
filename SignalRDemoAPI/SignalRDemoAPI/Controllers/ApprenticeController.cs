using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRDemoAPI.Models;

namespace SignalRDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprenticeController : ControllerBase
    {
        private readonly IHubContext<ApprenticeHub> signalrApprenticeHub;

        private static List<Apprentice> apprentices = new()
        {
            new Apprentice()
                {Name = "Student 1", Id = Guid.NewGuid(), ComprehensionLevel = MaterialComprehension.StrongComprehension},
            new Apprentice()
                {Name = "Student 2", Id = Guid.NewGuid(), ComprehensionLevel = MaterialComprehension.LittleComprehension},
            new Apprentice()
                {Name = "Student 3", Id = Guid.NewGuid(), ComprehensionLevel = MaterialComprehension.ModerateComprehension},
            new Apprentice()
                {Name = "Student 4", Id = Guid.NewGuid(), ComprehensionLevel = MaterialComprehension.ModerateComprehension},
            new Apprentice()
                {Name = "Student 5", Id = Guid.NewGuid(), ComprehensionLevel = MaterialComprehension.StrongComprehension},
        };

        public ApprenticeController(IHubContext<ApprenticeHub> signalrApprenticeHub)
        {
            this.signalrApprenticeHub = signalrApprenticeHub;
        }

        [HttpGet("")]
        public IActionResult GetAllApprentices()
        {
            signalrApprenticeHub.Clients.All.SendAsync("getallapprentices", apprentices);

            return Ok();
        }

        [HttpPut("")]
        public IActionResult UpdateApprentice(Apprentice apprentice)
        {
            var apprenticeToUpdate = apprentices.FirstOrDefault(a => a.Id == apprentice.Id);

            if (apprenticeToUpdate == null)
            {
                return BadRequest();
            }

            apprenticeToUpdate.Name = apprentice.Name;
            apprenticeToUpdate.ComprehensionLevel = apprentice.ComprehensionLevel;

            signalrApprenticeHub.Clients.All.SendAsync("getallapprentices", apprentices);
            
            return Ok(apprenticeToUpdate);
        }

        [HttpPost("")]
        public IActionResult AddApprentice(string apprenticeName)
        {
            var newApprentice = new Apprentice()
            {
                Name = apprenticeName, 
                Id = Guid.NewGuid(),
                ComprehensionLevel = MaterialComprehension.LittleComprehension
            };
            
            apprentices.Add(newApprentice);

            return Ok(newApprentice);
        }

        [HttpDelete("{apprenticeId:guid}")]
        public IActionResult DeleteApprentice(Guid apprenticeId)
        {
            var apprenticeToDelete = apprentices.FirstOrDefault(a => a.Id == apprenticeId);

            if (apprenticeToDelete == null)
            {
                return BadRequest();
            }
            
            apprentices.Remove(apprenticeToDelete);
            
            return Ok();
        }
    }
}