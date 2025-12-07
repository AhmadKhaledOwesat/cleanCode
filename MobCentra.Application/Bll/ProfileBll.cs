using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;
using System.Linq.Expressions;
using Profile = MobCentra.Domain.Entities.Profile;

namespace MobCentra.Application.Bll
{
    /// <summary>
    /// Business logic layer for profile management operations
    /// </summary>
    public class ProfileBll(IBaseDal<Profile, Guid, ProfileFilter> baseDal, IDeviceQueuBll deviceQueuBll, IConstraintBll constraintBll, IProfileFeatureBll profileFeatureBll, IDeviceBll deviceBll) : BaseBll<Profile, Guid, ProfileFilter>(baseDal), IProfileBll
    {
        /// <summary>
        /// Retrieves profiles with filtering by keyword (name in Arabic or Other), company, and active status
        /// </summary>
        /// <param name="searchParameters">Filter parameters for searching and pagination</param>
        /// <returns>Paginated result containing matching profiles</returns>
        public override Task<PageResult<Profile>> GetAllAsync(ProfileFilter searchParameters)
        {
            // Build search expression with keyword, company, and active status filters
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Profile, bool>(a =>

                (searchParameters.Keyword.IsNullOrEmpty() || a.NameAr.Contains(searchParameters?.Keyword) || a.NameOt.Contains(searchParameters?.Keyword))
                && a.CompanyId == searchParameters.CompanyId
                && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }

            return base.GetAllAsync(searchParameters);
        }

        /// <summary>
        /// Adds a new profile after validating company profile limits
        /// </summary>
        /// <param name="entity">The profile entity to add</param>
        public override async Task AddAsync(Profile entity)
        {
            // Check if company has reached the maximum number of profiles limit
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfProfile);
            await base.AddAsync(entity);
        }
        
        /// <summary>
        /// Retrieves the profile associated with a specific device
        /// </summary>
        /// <param name="id">The device identifier</param>
        /// <returns>The profile associated with the device</returns>
        public async Task<Profile> GetProfileByDeviceIdAsync(Guid id)
        {
            // Get device to retrieve profile ID
            Device device = await deviceBll.GetByIdAsync(id);
            return await GetByIdAsync(device.ProfileId.GetValueOrDefault());
        }
        
        /// <summary>
        /// Updates a profile, handles profile features, and queues devices for profile refresh
        /// </summary>
        /// <param name="entity">The profile entity with updated information</param>
        public override async Task UpdateAsync(Profile entity)
        {
            // Update profile features before updating profile
            await HandleFeatures(entity);
            await base.UpdateAsync(entity);
            
            // Queue all devices using this profile for refresh
            var deviceIds = (await deviceBll.FindAllByExpressionAsync(a => a.ProfileId == entity.Id)).Select(a => a.Id).ToArray();
            List<DeviceQueu> deviceQueus = [.. deviceIds.Select(a => new DeviceQueu() { DeviceId = a })];
            foreach (var item in deviceQueus)
            {
                // Add to queue if not already queued
                if (await deviceQueuBll.FindByExpressionAsync(a => a.DeviceId == item.DeviceId) is null)
                {
                    await deviceQueuBll.AddAsync(item);
                }
            }
        }
        
        /// <summary>
        /// Handles updating profile features by removing existing features and adding new ones
        /// </summary>
        /// <param name="entity">The profile entity containing new features</param>
        private async Task HandleFeatures(Profile entity)
        {
            // Find all existing profile features
            Expression<Func<ProfileFeature, bool>> expression = x => x.ProfileId == entity.Id;
            List<ProfileFeature> profileFeatures = await profileFeatureBll.FindAllByExpressionAsync(expression);
            
            // Delete existing features if any
            if (profileFeatures.Count > 0)
                await profileFeatureBll.DeleteRangeAsync(profileFeatures);
            
            // Add new features
            await profileFeatureBll.AddRangeAsync([.. entity.ProfileFeatures]);


        }
    }
}
