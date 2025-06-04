using Wedding.Model.Domain;
using Wedding.Model.DTO;

namespace Wedding.Service.IService;

public interface IClosedXMLService
{
    Task<string> ExportCustomerExcel(List<CustomerFullInfoDTO> customerFullInfoDTOs);
}