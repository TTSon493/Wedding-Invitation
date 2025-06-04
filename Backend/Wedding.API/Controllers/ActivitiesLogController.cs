using Wedding.DataAccess.IRepository;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedding.API.Controllers
{
    [Route("api/activities")]
    [ApiController]
    [Authorize(Roles = StaticUserRoles.Customer)]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllActivityLogs(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var responseDto = await _activityLogService.GetAll(User, filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> GetActivityLogById(Guid id)
        {
            var responseDto = await _activityLogService.GetById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateActivityLog(Guid id, UpdateActivityLogDTO updateActivityLogDTO)
        {
            var responseDto = await _activityLogService.UpdateById(updateActivityLogDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteActivityLog(Guid id)
        {
            var responseDto = await _activityLogService.DeleteById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateEmailTemplate(CreateActivityLogDTO createActivityLogDTO)
        {
            var responseDto = await _activityLogService.CreateById(createActivityLogDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}