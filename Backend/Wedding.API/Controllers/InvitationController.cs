using Wedding.DataAccess.IRepository;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedding.API.Controllers
{
    [Route("api/invitation")]
    [ApiController]
    [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Customer)]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllInvitations(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var responseDto = await _invitationService.GetAll(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> GetInvitationById(Guid id)
        {
            var responseDto = await _invitationService.GetById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateInvitation(Guid id, UpdateInvitationDTO updateInvitationDTO)
        {
            var responseDto = await _invitationService.UpdateById(id,updateInvitationDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteInvitation(Guid id)
        {
            var responseDto = await _invitationService.DeleteById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateInvitation(CreateInvitationDTO createInvitationDTO)
        {
            var responseDto = await _invitationService.CreateById(createInvitationDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        [Route("background/{InvationId:guid}")]
        public async Task<ActionResult<ResponseDTO>> UploadInvationBackground([FromRoute] Guid InvationId, UploadInvationBackgroundImg uploadInvationBackgroundImg)
        {
            var responseDto = await _invitationService.UploadInvationBackground(InvationId, uploadInvationBackgroundImg);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet]
        [Route("background/{InvationId:guid}")]
        public async Task<ActionResult> GetInvationBackground([FromRoute] Guid InvationId)
        {
            var responseDto = await _invitationService.GetInvationBackground(InvationId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}