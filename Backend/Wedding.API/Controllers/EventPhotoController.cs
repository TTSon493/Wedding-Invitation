using Wedding.DataAccess.IRepository;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedding.API.Controllers
{
    [Route("api/event-photo")]
    [ApiController]
    [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Customer)]
    public class EventPhotoController : ControllerBase
    {
        private readonly IEventPhotoService _EventPhotoService;

        public EventPhotoController(IEventPhotoService EventPhotoService)
        {
            _EventPhotoService = EventPhotoService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllEventPhotos(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var responseDto = await _EventPhotoService.GetAll(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> GetEventPhotoById(Guid id)
        {
            var responseDto = await _EventPhotoService.GetById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateEventPhoto(Guid id, UpdateEventPhotoDTO updateEventPhotoDTO)
        {
            var responseDto = await _EventPhotoService.UpdateById(id, updateEventPhotoDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteEventPhoto(Guid id)
        {
            var responseDto = await _EventPhotoService.DeleteById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateEmailTemplate(CreateEventPhotoDTO createEventPhotoDTO)
        {
            var responseDto = await _EventPhotoService.CreateById(createEventPhotoDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        [Route("background/{EventPhotoId:guid}")]
        public async Task<ActionResult<ResponseDTO>> UploadInvationTeamplateBackground([FromRoute] Guid EventPhotoId, UploadEventPhotoBackgroundImg uploadEventPhotoBackgroundImg)
        {
            var responseDto = await _EventPhotoService.UploadEventPhotoBackground(EventPhotoId, uploadEventPhotoBackgroundImg);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet]
        [Route("background/{EventPhotoId:guid}")]
        public async Task<ActionResult> DisplayEventPhotoBackground([FromRoute] Guid EventPhotoId)
        {
            var responseDto = await _EventPhotoService.GetEventPhotoBackground(EventPhotoId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}