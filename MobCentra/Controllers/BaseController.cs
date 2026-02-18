using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;

namespace MobCentra.Controllers
{
    [Authorize]
    public class BaseController<T, TDto, TId, TFilter>(IBaseBll<T, TId, TFilter> _baseBll, IDcpMapper _mapper) : Controller
        where T : BaseEntity<TId>
        where TDto : BaseDto<TId>
        where TId : struct
    {
        protected IDcpMapper mapper = _mapper;
        protected IBaseBll<T, TId, TFilter> baseBll = _baseBll;

        [HttpPost]
        [Route("")]
        [DisableRequestSizeLimit]
        public virtual async Task<DcpResponse<TId>> AddAsync([FromBody] TDto dto)
        {
            T entity = mapper.Map<T>(dto);
            await baseBll.AddAsync(entity);
            return new DcpResponse<TId>(entity.Id);
        }

        [HttpPost]
        [Route("range")]
        [DisableRequestSizeLimit]
        public virtual async Task<DcpResponse<List<TId>>> AddRangeAsync([FromBody] List<TDto> dtos)
        {
            List<T> entities = mapper.Map<List<T>>(dtos);
            await baseBll.AddRangeAsync(entities);
            return new DcpResponse<List<TId>>([.. entities.Select(a => a.Id)]);
        }

        [HttpPost]
        [Route("update")]
        [DisableRequestSizeLimit]
        public virtual async Task<DcpResponse<TId>> UpdateAsync([FromBody] TDto dto)
        {
            T entity = mapper.Map<T>(dto);
            await baseBll.UpdateAsync(entity);
            return new DcpResponse<TId>(entity.Id);
        }
        [HttpDelete]
        [Route("{id}")]
        public virtual async Task<DcpResponse<bool>> DeleteAsync([FromRoute] TId id) => new DcpResponse<bool>(await baseBll.DeleteAsync(id));
        [HttpGet]
        [Route("{id}")]
        public virtual async Task<DcpResponse<TDto>> GetByIdAsync([FromRoute] TId id) => new DcpResponse<TDto>(mapper.Map<TDto>(await baseBll.GetByIdAsync(id)));

        [HttpGet]
        [Route("all")]
        public virtual async Task<DcpResponse<List<TDto>>> GetAllAsync() => new DcpResponse<List<TDto>>(mapper.Map<List<TDto>>(await baseBll.GetAllAsync()));

        [HttpPost]
        [Route("search")]
        public virtual async Task<DcpResponse<PageResult<TDto>>> GetAllAsync([FromBody] TFilter searchParameters) => new DcpResponse<PageResult<TDto>>(mapper.Map<PageResult<TDto>>(await baseBll.GetAllAsync(searchParameters)));
    }
}
