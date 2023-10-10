using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers.Base
{
    [Route("api/[controller]")]
    public class VpsCRUDController<T> : VpsController<T>
        where T : class
    {
        public VpsCRUDController(IVpsRepository<T> repository)
            : base(repository) { }

        [HttpGet]
        public IEnumerable<T> GetList()
        {
            return this.vpsRepository.Entities.AsEnumerable();
        }
        [HttpGet("{id}")]
        public async Task<T> GetById(Guid id)
        {
            return await this.vpsRepository.Find(id);
        }
        [HttpPost()]
        public async Task<T> Create(T data)
        {
            T created = await this.vpsRepository.Create(data);
            await this.vpsRepository.SaveChange();
            return created;
        }
        [HttpPut]
        public async Task<T> Update(T data)
        {
            T updated = await this.vpsRepository.Update(data);
            await this.vpsRepository.SaveChange();
            return updated;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            await this.vpsRepository.Delete<Guid>(id);
            return NoContent();
        }
    }
}
