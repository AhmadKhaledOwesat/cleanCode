using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class TransactionBll(IBaseDal<Transaction, Guid, TransactionFilter> baseDal) : BaseBll<Transaction, Guid, TransactionFilter>(baseDal), ITransactionBll
    {

        public override Task<PageResult<Transaction>> GetAllAsync(TransactionFilter searchParameters)
        {
            if (searchParameters is not null)
            {
                searchParameters.Expression = new Func<Transaction, bool>(a =>
                a.CompanyId == searchParameters?.CompanyId &&
                (searchParameters.AppUserId == null || searchParameters.AppUserId == a.AppUserId) &&
                (!searchParameters.FromDate.HasValue || searchParameters.FromDate.Value.Date >= a.TransDateTime.Date) &&
                (!searchParameters.ToDate.HasValue || searchParameters.ToDate.Value.Date <= a.TransDateTime.Date));
            }

            return base.GetAllAsync(searchParameters);
        }
    }
}
