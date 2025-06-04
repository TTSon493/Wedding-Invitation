using System.Security.Claims;
using Wedding.Model.DTO;
using Wedding.Model.Domain;

namespace Wedding.Service.IService;

public interface IEventService
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
    Task<ResponseDTO> UpdateById(Guid id, UpdateEventDTO updateEventDTO);
    Task<ResponseDTO> DeleteById(Guid id);
    Task<ResponseDTO> CreateById(CreateEventDTO createEventDTO);
    Task<ResponseDTO> UploadEventBackground(Guid EventId, UploadEventBackgroundImg uploadEventBackgroundImg);
    Task<ResponseDTO> GetEventBackground( Guid EventId);
}