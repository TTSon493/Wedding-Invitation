using Wedding.Model.DTO;
using System.Security.Claims;

namespace Wedding.Service.IService
{
    public interface ICustomersService
    {
        Task<ResponseDTO> GetAllCustomer
    (
        ClaimsPrincipal User,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        bool? isAscending,
        int pageNumber,
        int pageSize
    );

        Task<ResponseDTO> GetById(Guid id);
        Task<ResponseDTO> UpdateById(UpdateCustomerDTO updateCustomerDTO);
        Task<ResponseDTO> ActivateCustomer(ClaimsPrincipal User, Guid customerId);
        Task<ResponseDTO> DeactivateCustomer(ClaimsPrincipal User, Guid customerId);
        Task<ResponseDTO> ExportCustomers(string userId, int month, int year);
        Task<ClosedXMLResponseDTO> DownloadCustomers(string fileName);
    }
}