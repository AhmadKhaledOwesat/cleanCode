using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using DcpTracker.Infrastructure.Extensions;

namespace DcpTracker.Application.Bll
{
    public class ApplicationBll(IBaseDal<DcpTracker.Domain.Entities.Application, Guid, ApplicationFilter> baseDal, IConstraintBll constraintBll) : BaseBll<DcpTracker.Domain.Entities.Application, Guid, ApplicationFilter>(baseDal), IApplicationBll
    {
        public override async Task<PageResult<DcpTracker.Domain.Entities.Application>> GetAllAsync(ApplicationFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<DcpTracker.Domain.Entities.Application, bool>(a =>
                (a.NameAr == searchParameters?.Description || searchParameters.Description.IsNullOrEmpty())
                && a.CompanyId == searchParameters.CompanyId
                && (searchParameters.Active == null || a.Active == searchParameters.Active));
            }
            return await base.GetAllAsync(searchParameters);
        }

        public override async Task AddAsync(Domain.Entities.Application entity)
        {
            await constraintBll.GetLimitAsync(entity.CompanyId.Value, Domain.Enum.LimitType.NoOfApplications);

            if (!entity.File.IsNullOrEmpty())
                entity.File = await entity.File.UplodaFiles();
            await base.AddAsync(entity);
        }

        public override async Task UpdateAsync(Domain.Entities.Application entity)
        {
            var app = await GetByIdAsync(entity.Id);

            if (app.File != entity.File && !entity.File.IsNullOrEmpty())
                entity.File = await app.File.UplodaFiles();
            else
                entity.File = app.File;
            await base.UpdateAsync(entity);
        }
    }
}
