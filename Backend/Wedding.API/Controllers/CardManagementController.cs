using Wedding.DataAccess.IRepository;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedding.API.Controllers
{
    [Route("api/card-management")]
    [ApiController]
    [Authorize(Roles = StaticUserRoles.Customer)]
    public class CardManagementController : ControllerBase
    {
        private readonly ICardManagementService _cardManagementService;

        public CardManagementController(ICardManagementService cardManagementService)
        {
            _cardManagementService = cardManagementService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllCardManagements(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var responseDto = await _cardManagementService.GetAll(User, filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> GetCardManagementById(Guid id)
        {
            var responseDto = await _cardManagementService.GetById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> UpdateCardManagement(Guid id, UpdateCardManagementDTO updateCardManagementDTO)
        {
            var responseDto = await _cardManagementService.UpdateById(updateCardManagementDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteCardManagement(Guid id)
        {
            var responseDto = await _cardManagementService.DeleteById(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateCardManagement(CreateCardManagementDTO createCardManagementDTO)
        {
            var responseDto = await _cardManagementService.CreateById(createCardManagementDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}