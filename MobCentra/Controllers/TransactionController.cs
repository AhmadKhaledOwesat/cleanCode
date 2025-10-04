using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MobCentra.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class TransactionController(ITransactionBll cityBll, IDcpMapper mapper) : BaseController<Transaction, TransactionDto, Guid, TransactionFilter>(cityBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<TransactionDto>>> GetAllAsync([FromBody] TransactionFilter searchParameters)=> new DcpResponse<PageResult<TransactionDto>>(mapper.Map<PageResult<TransactionDto>>(await cityBll.GetAllAsync(searchParameters)));
    }
}
