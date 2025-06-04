using System.Security.Claims;
using Wedding.Model.DTO;
using Wedding.Model.Domain;

namespace Wedding.Service.IService;

public interface IEventPhotoService
{
    Task<ResponseDTO> GetAll
    (
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        bool? isAscending,
        int pageNumber,
        int pageSize
    );

    Task<ResponseDTO> GetById(Guid id);
    Task<ResponseDTO> UpdateById(Guid id, UpdateEventPhotoDTO updateEventPhotoDTO);
    Task<ResponseDTO> DeleteById(Guid id);
    Task<ResponseDTO> CreateById(CreateEventPhotoDTO createEventPhotoDTO);
    Task<ResponseDTO> UploadEventPhotoBackground(Guid EventPhotoId, UploadEventPhotoBackgroundImg uploadEventPhotoBackgroundImg);
    Task<ResponseDTO> GetEventPhotoBackground( Guid EventPhotoId);
}