using Wedding.Model.DTO;

namespace Wedding.Service.IService;

public interface IBaseService
{
    Task<ResponseDTO?> SendAsync(RequestDTO requestDto, string? apiKey);
}
