using CampusStay.Brokers.Storages;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace CampusStay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : RESTFulController
    {
        private readonly IStorageBroker storageBroker;

        public HomeController(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }
        [HttpGet]
        public string SayHello()
        {
            return "hi";
        }
    }
}
