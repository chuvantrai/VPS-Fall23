using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.BrokerApi.Models;
using Service.BrokerApi.Models.SmtpJob;
using Service.BrokerApi.Services;

namespace Service.BrokerApi.Controllers
{
    [Route("api/smtp")]
    public class SmtpBrokerController : ApiBrokerController<IRabbitMQClient>
    {
        public SmtpBrokerController(IRabbitMQClient rabbitMQClient, IOptions<RabbitMQProfile> options) : base(rabbitMQClient, options)
        {
        }
        [HttpPost("send-mail")]
        public async Task<IActionResult> SendMail(SmtpMessageDto smtpMessageDto)
        {
            await this.rabbitMQClient.SendMessageAsync<SmtpMessageDto>(this.rabbitMQProfile.QueueInfo.SmtpQueueName, smtpMessageDto);
            return Ok();
        }
    }
}
