using Wedding.DataAccess.IRepository;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedding.API.Controllers
{
    [Route("api/invitation-html")]
    [ApiController]
    [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Customer)]
    public class InvitationHtmlController : ControllerBase
    {
        private readonly IInvitationHtmlService _invitationHtmlService;

        public InvitationHtmlController(IInvitationHtmlService invitationHtmlService)
        {
            _invitationHtmlService = invitationHtmlService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllInvitationHtml(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var responseDto = await _invitationHtmlService.GetAll(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> GetInvitationHtmlById(Guid id)
        {
            var responseDto = await _invitationHtmlService.GetById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateInvitationHtml(Guid id, UpdateInvitationHtmlDTO updateInvitationHtmlDTO)
        {
            var responseDto = await _invitationHtmlService.UpdateById(id,updateInvitationHtmlDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteInvitationHtml(Guid id)
        {
            var responseDto = await _invitationHtmlService.DeleteById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateInvitation(CreateInvitationHtmlDTO createInvitationHtmlDTO)
        {
            var responseDto = await _invitationHtmlService.CreateById(createInvitationHtmlDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}