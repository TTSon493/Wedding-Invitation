using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.Service.Service;
using Wedding.Utility.Constants;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Wedding.API.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomersService _customersService;
        public CustomerController(ICustomersService customersService)
        {
            _customersService = customersService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> GetAllCustomer
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var responseDto =
                await _customersService.GetAllCustomer(User, filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet]
        [Route("{customerId:guid}")]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> GetCustomerById([FromRoute] Guid customerId)
        {
            var responseDto = await _customersService.GetById(customerId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRoles.AdminCustomer)]
        public async Task<ActionResult<ResponseDTO>> UpdateStudent([FromBody] UpdateCustomerDTO updateCustomerDTO)
        {
            var responseDto = await _customersService.UpdateById(updateCustomerDTO);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        [Route("activate/{customerId:guid}")]
        [Authorize(Roles = StaticUserRoles.Admin)]
        public async Task<ActionResult<ResponseDTO>> ActivateCustomer([FromRoute] Guid customerId)
        {
            var responseDto = await _customersService.ActivateCustomer(User, customerId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        [Route("deactivate/{customerId:guid}")]
        [Authorize(Roles = StaticUserRoles.Admin)]
        public async Task<ActionResult<ResponseDTO>> DeactiveCustomer([FromRoute] Guid customerId)
        {
            var responseDto = await _customersService.DeactivateCustomer(User, customerId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        [Route("export/{month:int}/{year:int}")]
        [Authorize(Roles = StaticUserRoles.Admin)]
        public async Task<ActionResult<ResponseDTO>> ExportCustomer
        (
            [FromRoute] int month,
            [FromRoute] int year
        )
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            BackgroundJob.Enqueue<ICustomersService>(job => job.ExportCustomers(userId, month, year));
            return Ok();
        }

        [HttpGet]
        [Route("download/{fileName}")]
        [Authorize(Roles = StaticUserRoles.Admin)]
        public async Task<IActionResult> DownloadCustomerExport([FromRoute] string fileName)
        {
            var closedXmlResponseDto = await _customersService.DownloadCustomers(fileName);
            var stream = closedXmlResponseDto.Stream;
            var contentType = closedXmlResponseDto.ContentType;

            if (stream is null || contentType is null)
            {
                return NotFound();
            }

            return File(stream, contentType, fileName);
        }
    }
}
