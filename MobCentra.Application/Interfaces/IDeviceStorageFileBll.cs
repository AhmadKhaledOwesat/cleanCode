using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;

namespace MobCentra.Domain.Interfaces
{
    public interface IDeviceStorageFileBll : IBaseBll<DeviceStorageFile, Guid, DeviceStorageFileFilter>
    {
        Task<DeviceStorageFile> GetFileByNameAsync(string name);
    }
}
