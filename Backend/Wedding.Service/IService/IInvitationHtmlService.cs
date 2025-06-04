using Wedding.Model.DTO;

namespace Wedding.Service.IService;

public interface IInvitationHtmlService
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
    Task<ResponseDTO> UpdateById(Guid id, UpdateInvitationHtmlDTO updateInvitationHtmlDTO);
    Task<ResponseDTO> DeleteById(Guid id);
    Task<ResponseDTO> CreateById(CreateInvitationHtmlDTO createInvitationHtmlDTO);
}