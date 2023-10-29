using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories
{
    public class PaymentTransactionRepository : VpsRepository<PaymentTransaction>, IPaymentTransactionRepository
    {
        public PaymentTransactionRepository(FALL23_SWP490_G14Context fALL23_SWP490_G14Context)
            : base(fALL23_SWP490_G14Context)
        {

        }
    }
}
