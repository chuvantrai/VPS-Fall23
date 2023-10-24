using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class ContractRepository : VpsRepository<Contract>, IContractRepository
{
    public ContractRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }
}