using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobCentra.Application.Dto;
using MobCentra.Domain.Entities;
using MobCentra.Domain.Entities.Filters;
using MobCentra.Domain.Interfaces;
using Group = MobCentra.Domain.Entities.Group;

namespace MobCentra.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class GroupController(IGroupBll groupBll, IDcpMapper mapper) : BaseController<Group, GroupDto, Guid, GroupFilter>(groupBll, mapper)
    {
        public override async Task<DcpResponse<PageResult<GroupDto>>> GetAllAsync([FromBody] GroupFilter searchParameters)
        {
            if (!searchParameters.IsByPass && !await groupBll.IsAuthorizedAsync(Guid.Parse(Permissions.DeviceGroup)))
                throw new UnauthorizedAccessException();

            return new DcpResponse<PageResult<GroupDto>>(mapper.Map<PageResult<GroupDto>>(await groupBll.GetAllAsync(searchParameters)));
        }
    }
}
