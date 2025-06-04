using Wedding.DataAccess.IRepository;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedding.API.Controllers
{
    [Route("api/invitation-template")]
    [ApiController]
    //[Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Customer)]
    public class InvitationTemplateController : ControllerBase
    {
        private readonly IInvitationTemplateService _invitationTemplateService;

        public InvitationTemplateController(IInvitationTemplateService invitationTemplateService)
        {
            _invitationTemplateService = invitationTemplateService;
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
            var responseDto = await _invitationTemplateService.GetAll(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> GetInvitationTemplateById(Guid id)
        {
            var responseDto = await _invitationTemplateService.GetById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateInvitationTemplate(Guid id, UpdateInvitationTemplateDTO updateInvitationTemplateDTO)
        {
            var responseDto = await _invitationTemplateService.UpdateById(id, updateInvitationTemplateDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteInvitationTemplate(Guid id)
        {
            var responseDto = await _invitationTemplateService.DeleteById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateInvitationTemplate(CreateInvitationTemplateDTO createInvitationTemplateDTO)
        {
            var responseDto = await _invitationTemplateService.CreateById(createInvitationTemplateDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        [Route("background/{TemplateId:guid}")]
        public async Task<ActionResult<ResponseDTO>> UploadInvationTeamplateBackground([FromRoute] Guid TemplateId, UploadInvationTeamplateBackgroundImg uploadInvationTemplateBackgroundImg)
        {
            var responseDto = await _invitationTemplateService.UploadInvationTeamplateBackgroundImg(TemplateId, uploadInvationTemplateBackgroundImg);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet]
        [Route("background/{TemplateId:guid}")]
        public async Task<ActionResult> GetInvationTeamplateBackgrounds([FromRoute] Guid TemplateId)
        {
            var responseDto = await _invitationTemplateService.GetInvationTeamplateBackgrounds(TemplateId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}