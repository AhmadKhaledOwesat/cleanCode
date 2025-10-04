using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Enum;
using MobCentra.Domain.Interfaces;
using MobCentra.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;
namespace MobCentra.Application.Bll
{
    public class ConstraintBll(IBaseDal<Domain.Entities.Application, Guid, ApplicationFilter> baseDal, IServiceProvider serviceProvider) : BaseBll<Domain.Entities.Application, Guid, ApplicationFilter>(baseDal), IConstraintBll
    {
        public async Task<bool> GetLimitAsync(Guid companyId, LimitType limitType)
        {
            var companyBll = serviceProvider.GetService<ICompanyBll>();
            var company = await companyBll.GetByIdAsync(companyId);
            int companyLimit = Convert.ToInt32(company.GetType().GetProperty(limitType.ToString())?.GetValue(company) ?? 0);
            var typeMap = new Dictionary<LimitType, Type>
            {
                [LimitType.NoOfUsers] = typeof(Users),
                [LimitType.NoOfDevices] = typeof(Device),
                [LimitType.NoOfTrackedDevices] = typeof(Device),
                [LimitType.NoOfRoles] = typeof(Role),
                [LimitType.NoOfGroups] = typeof(Group),
                [LimitType.NoOfArea] = typeof(City),
                [LimitType.NoOfProfile] = typeof(Profile),
                [LimitType.NoOfNotifications] = typeof(Users),
                [LimitType.NoOfApplications] = typeof(Domain.Entities.Application),
            };

            int currentLimit = 0;
            if (typeMap.TryGetValue(limitType, out var classType))
            {
                var getRecordCountMethod = typeof(ConstraintBll).GetMethod(nameof(GetRecordCountAsync), BindingFlags.NonPublic | BindingFlags.Instance)!.MakeGenericMethod(classType);
                currentLimit = await (Task<int>)getRecordCountMethod.Invoke(this, [companyId]);
            }
            return companyLimit < currentLimit + 1 ? throw new Exception($"لا يمكن اضافة سجل جديد لانك تجازوت الحد الاقصى من السجلات") : true;
        }

       
        private static Expression<Func<T, bool>> BuildCompanyPredicate<T>(Guid companyId)
        {
            var param = Expression.Parameter(typeof(T), "e");
            var companyIdProperty = Expression.Property(param, "CompanyId");
            var companyIdValue = Expression.Constant(companyId, typeof(Guid?));
            var body = Expression.Equal(companyIdProperty, companyIdValue);
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
        private async Task<int> GetRecordCountAsync<T>(Guid companyId) where T : BaseEntity<Guid>
        {
            IEfRepository<T, Guid> efRepository = serviceProvider.GetService<IEfRepository<T, Guid>>();
            Expression<Func<T, bool>> predicate = BuildCompanyPredicate<T>(companyId);
            return await efRepository.GetCountAsync(predicate);
        }
    }
}
