using Microsoft.AspNetCore.Mvc;
using Service.BrokerApi.Services;

namespace Service.BrokerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBrokerController<T> : ControllerBase
        where T : RabbitMQClient
    {
        protected readonly RabbitMQClient rabbitMQClient;
        public ApiBrokerController(T rabbitMQClient)
        {
            this.rabbitMQClient = rabbitMQClient;
        }
    }
}
