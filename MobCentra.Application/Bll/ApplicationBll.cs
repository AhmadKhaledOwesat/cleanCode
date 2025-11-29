using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Extensions;

namespace MobCentra.Application.Bll
{
    public class ApplicationBll(IBaseDal<MobCentra.Domain.Entities.Application, Guid, ApplicationFilter> baseDal, IConstraintBll constraintBll) : BaseBll<MobCentra.Domain.Entities.Application, Guid, ApplicationFilter>(baseDal), IApplicationBll
    {
        public override async Task<PageResult<MobCentra.Domain.Entities.Application>> GetAllAsync(ApplicationFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<MobCentra.Domain.Entities.Application, bool>(a =>
                    (searchParameters.Keyword.IsNullOrEmpty() || a.NameAr.Contains(searchParameters?.Keyword))
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
