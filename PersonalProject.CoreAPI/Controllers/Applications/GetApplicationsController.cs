using Microsoft.AspNetCore.Mvc;

namespace PersonalProject.CoreAPI.Controllers.Applications
{
    [ApiController]
    [Route("[controller]")]
    public class GetApplicationsController : ControllerBase
    {
        private readonly ILogger<GetApplicationsController> _logger;

        public GetApplicationsController(ILogger<GetApplicationsController> logger)
        {
            _logger = logger;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return 
        //}
    }
}
