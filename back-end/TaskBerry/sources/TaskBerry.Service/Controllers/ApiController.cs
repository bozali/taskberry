namespace TaskBerry.Service.Controllers
{
    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("/api")]
    public class ApiController
    {
        [HttpGet("/api/version")]
        [Produces("application/json")]
        public ActionResult<string> GetVersion()
        {
            return "TaskBerry v1";
        }
    }
}