using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using System.Linq.Expressions;
using Profile = MobCentra.Domain.Entities.Profile;

namespace MobCentra.Application.Bll
{
    public class ProfileBll(IBaseDal<Profile, Guid, ProfileFilter> baseDal, IDeviceQueuBll deviceQueuBll, IConstraintBll constraintBll, IProfileFeatureBll profileFeatureBll, IDeviceBll deviceBll) : BaseBll<Profile, Guid, ProfileFilter>(baseDal), IProfileBll
    {
        public override Task<PageResult<Profile>> GetAllAsync(ProfileFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Profile, bool>(a =>

                (a.NameAr == searchParameters?.Description || searchParameters.Description.IsNullOrEmpty())
                && a.CompanyId == searchParameters.CompanyId
                && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }

        public override async Task AddAsync(Profile entity)
        {
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfProfile);
            await base.AddAsync(entity);
        }
        public async Task<Profile> GetProfileByDeviceIdAsync(Guid id)
        {
            Device device = await deviceBll.GetByIdAsync(id);
            return await GetByIdAsync(device.ProfileId.GetValueOrDefault());
        }
        public override async Task UpdateAsync(Profile entity)
        {
            await HandleFeatures(entity);
            await base.UpdateAsync(entity);
            var deviceIds = (await deviceBll.FindAllByExpressionAsync(a => a.ProfileId == entity.Id)).Select(a => a.Id).ToArray();
            List<DeviceQueu> deviceQueus = [.. deviceIds.Select(a => new DeviceQueu() { DeviceId = a })];
            foreach (var item in deviceQueus)
            {
                if (await deviceQueuBll.FindByExpressionAsync(a => a.DeviceId == item.DeviceId) is null)
                {
                    await deviceQueuBll.AddAsync(item);
                }
            }
        }
        private async Task HandleFeatures(Profile entity)
        {
            Expression<Func<ProfileFeature, bool>> expression = x => x.ProfileId == entity.Id;
            List<ProfileFeature> profileFeatures = await profileFeatureBll.FindAllByExpressionAsync(expression);
            if (profileFeatures.Count > 0)
                await profileFeatureBll.DeleteRangeAsync(profileFeatures);
            await profileFeatureBll.AddRangeAsync([.. entity.ProfileFeatures]);


        }
    }
}
