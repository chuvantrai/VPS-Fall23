using Microsoft.AspNetCore.SignalR;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.SignalR
{
    public class PaymentHub : Hub
    {
        readonly IPaymentTransactionRepository paymentTransactionRepository;
        public PaymentHub(IPaymentTransactionRepository paymentTransactionRepository)
        {
            this.paymentTransactionRepository = paymentTransactionRepository;
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public async Task ConfirmPaid(string connectionId, string message)
        {
            await this.Clients.Client(connectionId).SendAsync("ReceivePaidStatus", message);
        }
        public async Task RegisterPayment(string connectionId, string txnRef)
        {
            var paymentTransaction = await this.paymentTransactionRepository.Find(txnRef);
            paymentTransaction.ConnectionId = connectionId;
            await this.paymentTransactionRepository.Update(paymentTransaction);
            await this.paymentTransactionRepository.SaveChange();
        }
    }
}
