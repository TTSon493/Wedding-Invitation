using Wedding.Model.DTO;

namespace Wedding.Service.IService;

public interface ICompanyService
{
    Task<ResponseDTO> GetCompany();
    Task<ResponseDTO> UpdateCompany(UpdateCompanyDTO companyDto);
}