using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.BrokerApi.Models;
using Service.BrokerApi.Services;

namespace Service.BrokerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBrokerController<T> : ControllerBase
        where T : IRabbitMQClient
    {
        protected readonly RabbitMQProfile rabbitMQProfile;
        protected readonly IRabbitMQClient rabbitMQClient;
        public ApiBrokerController(T rabbitMQClient, IOptions<RabbitMQProfile> options)
        {
            this.rabbitMQProfile = options.Value;
            this.rabbitMQClient = rabbitMQClient;
        }
    }
}
