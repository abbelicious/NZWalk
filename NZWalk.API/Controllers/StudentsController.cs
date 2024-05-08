using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalk.API.Controllers
{
    // this should be called on https://localhost:portnumber/api/Students
    // portnumber is set in launchSettings.json
    // students is set from [controller] bellow
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET: https://localhost:portnumber/api/Students
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentsNames = [ "Pelle", "Janne", "Emelie", "David", "Johanna" ];
            return Ok(studentsNames);
        }
    }
}
