using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRDemoAPI.Models;

namespace SignalRDemoAPI.Controllers
{
    [Route("api/[controller]")]
    public class ApprenticeController : ControllerBase
    {
        private readonly IHubContext<ApprenticeHub> signalrApprenticeHub;

        private static List<Apprentice> apprentices = new()
        {
            new Apprentice()
                {Name = "Student 1", Id = 1, ContentComprehension = ComprehensionLevel.StrongComprehension},
            new Apprentice()
                {Name = "Student 2", Id = 2, ContentComprehension = ComprehensionLevel.LittleComprehension},
            new Apprentice()
                {Name = "Student 3", Id = 3, ContentComprehension = ComprehensionLevel.ModerateComprehension},
            new Apprentice()
                {Name = "Student 4", Id = 4, ContentComprehension = ComprehensionLevel.ModerateComprehension},
            new Apprentice()
                {Name = "Student 5", Id = 5, ContentComprehension = ComprehensionLevel.StrongComprehension},
        };

        public ApprenticeController(IHubContext<ApprenticeHub> signalrApprenticeHub)
        {
            this.signalrApprenticeHub = signalrApprenticeHub;
        }

        [HttpGet("")]
        public IActionResult GetAllApprentices()
        {
            return Ok(apprentices);
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
            apprenticeToUpdate.ContentComprehension = apprentice.ContentComprehension;

            // This is what sends the updated apprentices to the subscribed clients
            signalrApprenticeHub.Clients.All.SendAsync("getallapprentices", apprentices);
            
            return Ok(apprenticeToUpdate);
        }

        [HttpPost("")]
        public IActionResult AddApprentice(string apprenticeName)
        {
            var newApprentice = new Apprentice()
            {
                Name = apprenticeName, 
                Id = apprentices.Count + 1,
                ContentComprehension = ComprehensionLevel.LittleComprehension
            };
            
            apprentices.Add(newApprentice);
            
            // This is what sends the updated apprentices to the subscribed clients
            signalrApprenticeHub.Clients.All.SendAsync("getallapprentices", apprentices);
            
            return Ok(newApprentice);
        }

        [HttpDelete("{apprenticeId:int}")]
        public IActionResult DeleteApprentice(int apprenticeId)
        {
            var apprenticeToDelete = apprentices.FirstOrDefault(a => a.Id == apprenticeId);

            if (apprenticeToDelete == null)
            {
                return BadRequest();
            }
            
            apprentices.Remove(apprenticeToDelete);
            
            signalrApprenticeHub.Clients.All.SendAsync("getallapprentices", apprentices);

            return Ok();
        }
    }
}
