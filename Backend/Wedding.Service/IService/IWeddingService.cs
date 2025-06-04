using System.Security.Claims;
using Wedding.Model.DTO;
using Wedding.Model.Domain;

namespace Wedding.Service.IService;

public interface IWeddingService
{
    Task<ResponseDTO> GetAll
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
    Task<ResponseDTO> UpdateById(Guid id, UpdateWeddingDTO updateWeddingDTO);
    Task<ResponseDTO> DeleteById(Guid id);
    Task<ResponseDTO> CreateById(CreateWeddingDTO createWeddingDTO);
    Task<ResponseDTO> UploadWeddingBackground(Guid WeddingId, UploadWeddingBackgroundImg uploadCourseVersionBackgroundImg);
    Task<ResponseDTO> GetWeddingBackground( Guid WeddingId);
}