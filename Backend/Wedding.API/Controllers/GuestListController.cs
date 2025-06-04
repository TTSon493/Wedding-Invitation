using Wedding.DataAccess.IRepository;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedding.API.Controllers
{
    [Route("api/guest-list")]
    [ApiController]
    [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Customer)]
    public class GuestListController : ControllerBase
    {
        private readonly IGuestListService _GuestListService;

        public GuestListController(IGuestListService GuestListService)
        {
            _GuestListService = GuestListService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllGuestLists(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var responseDto = await _GuestListService.GetAll(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> GetGuestListById(Guid id)
        {
            var responseDto = await _GuestListService.GetById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateGuestList(Guid id, UpdateGuestListDTO updateGuestListDTO)
        {
            var responseDto = await _GuestListService.UpdateById(id, updateGuestListDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteGuestList(Guid id)
        {
            var responseDto = await _GuestListService.DeleteById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateEmailTemplate(CreateGuestListDTO createGuestListDTO)
        {
            var responseDto = await _GuestListService.CreateById(createGuestListDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}