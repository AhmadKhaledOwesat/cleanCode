using DcpTracker.Application.Dto;
using DcpTracker.Domain.Entities;
using DcpTracker.Domain.Entities.Filters;
using DcpTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DcpTracker.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class TransactionController(ITransactionBll cityBll, IDcpMapper mapper) : BaseController<Transaction, TransactionDto, Guid, TransactionFilter>(cityBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<TransactionDto>>> GetAllAsync([FromBody] TransactionFilter searchParameters)=> new DcpResponse<PageResult<TransactionDto>>(mapper.Map<PageResult<TransactionDto>>(await cityBll.GetAllAsync(searchParameters)));
    }
}
