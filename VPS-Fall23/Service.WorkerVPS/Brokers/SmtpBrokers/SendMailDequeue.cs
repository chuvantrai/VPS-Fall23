using Microsoft.Extensions.Options;
using Service.WorkerVPS.Models;
using Service.WorkerVPS.Models.SmtpJob;
using Service.WorkerVPS.Services.Smtp;

namespace Service.WorkerVPS.Brokers.SmtpBrokers
{
    internal class SendMailDequeue : RabbitMQClient<SmtpMessageDto>
    {
        readonly ISmtpService smtpService;
        public SendMailDequeue(IOptions<RabbitMQProfile> rabbitMQProfile,
            ISmtpService smtpService, ILogger<SendMailDequeue> logger) : base(rabbitMQProfile.Value, rabbitMQProfile.Value.QueueInfo.SmtpQueueName, logger)
        {
            this.smtpService = smtpService;
        }

        public override async Task DequeueHandle(SmtpMessageDto message)
        {
            var smtpMessage = smtpService.CreateMailMessage(message.Subject, message.Message, message.Attachments);
            await smtpService.Send(smtpMessage);
        }
    }
}
