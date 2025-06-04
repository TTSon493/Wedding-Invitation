using Wedding.DataAccess.IRepository;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedding.API.Controllers
{
    [Route("api/event")]
    [ApiController]
    [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Customer)]
    public class EventController : ControllerBase
    {
        private readonly IEventService _EventService;

        public EventController(IEventService EventService)
        {
            _EventService = EventService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllEvents(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var responseDto = await _EventService.GetAll(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> GetEventById(Guid id)
        {
            var responseDto = await _EventService.GetById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateEvent(Guid id, UpdateEventDTO updateEventDTO)
        {
            var responseDto = await _EventService.UpdateById(id, updateEventDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteEvent(Guid id)
        {
            var responseDto = await _EventService.DeleteById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateEmailTemplate(CreateEventDTO createEventDTO)
        {
            var responseDto = await _EventService.CreateById(createEventDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        [Route("background/{EventId:guid}")]
        public async Task<ActionResult<ResponseDTO>> UploadInvationTeamplateBackground([FromRoute] Guid EventId, UploadEventBackgroundImg uploadEventBackgroundImg)
        {
            var responseDto = await _EventService.UploadEventBackground(EventId, uploadEventBackgroundImg);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet]
        [Route("background/{EventId:guid}")]
        public async Task<ActionResult> DisplayEventPhotoBackground([FromRoute] Guid EventId)
        {
            var responseDto = await _EventService.GetEventBackground(EventId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}