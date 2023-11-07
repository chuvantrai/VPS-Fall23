using Quartz;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Extensions.StaticLogic.Scheduler;

public class DeleteParkingZoneJob : IJob
{
    private readonly IParkingZoneRepository _parkingZoneRepository;
    private readonly IParkingZoneAbsentRepository _absentRepository;

    public DeleteParkingZoneJob(IParkingZoneRepository parkingZoneRepository,
        IParkingZoneAbsentRepository absentRepository)
    {
        _parkingZoneRepository = parkingZoneRepository;
        _absentRepository = absentRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Delete job start running!");
        var parkingZoneId = context.MergedJobDataMap.GetGuid("parkingZoneId");
        var parkingZone = _parkingZoneRepository.GetParkingZoneAndAbsentById(parkingZoneId);
        if (parkingZone is null)
        {
            throw new ServerException(2);
        }
        Console.WriteLine("Delete job running!");
        await _absentRepository.DeleteRange(
            (List<ParkingZoneAbsent>)parkingZone.ParkingZoneAbsents);
        await _parkingZoneRepository.Delete(parkingZone);
        await _parkingZoneRepository.SaveChange();
    }
}