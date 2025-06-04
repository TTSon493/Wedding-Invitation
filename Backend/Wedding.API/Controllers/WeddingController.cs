using Wedding.DataAccess.IRepository;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedding.API.Controllers
{
    [Route("api/wedding")]
    [ApiController]
    [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Customer)]
    public class WeddingController : ControllerBase
    {
        private readonly IWeddingService _weddingService;

        public WeddingController(IWeddingService weddingService)
        {
            _weddingService = weddingService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllWeddings(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var responseDto = await _weddingService.GetAll(User, filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> GetActivityLogById(Guid id)
        {
            var responseDto = await _weddingService.GetById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateActivityLog(Guid id, UpdateWeddingDTO updateWeddingDTO)
        {
            var responseDto = await _weddingService.UpdateById(id, updateWeddingDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteActivityLog(Guid id)
        {
            var responseDto = await _weddingService.DeleteById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateEmailTemplate(CreateWeddingDTO createWeddingDTO)
        {
            var responseDto = await _weddingService.CreateById(createWeddingDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        [Route("background/{WeddingId:guid}")]
        public async Task<ActionResult<ResponseDTO>> UploadWeddingBackground([FromRoute] Guid WeddingId, UploadWeddingBackgroundImg uploadWeddingBackgroundImg)
        {
            var responseDto = await _weddingService.UploadWeddingBackground(WeddingId, uploadWeddingBackgroundImg);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet]
        [Route("background/{WeddingId:guid}")]
        public async Task<ActionResult> GetWeddingBackground([FromRoute] Guid WeddingId)
        {
            var responseDto = await _weddingService.GetWeddingBackground(WeddingId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}