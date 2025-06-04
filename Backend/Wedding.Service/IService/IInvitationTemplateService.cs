using System.Security.Claims;
using Wedding.Model.DTO;
using Wedding.Model.Domain;

namespace Wedding.Service.IService;

public interface IInvitationTemplateService
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
    Task<ResponseDTO> UpdateById(Guid id, UpdateInvitationTemplateDTO updateInvitationTemplateDTO);
    Task<ResponseDTO> DeleteById(Guid id);
    Task<ResponseDTO> CreateById(CreateInvitationTemplateDTO createInvitationTemplateDTO);
    Task<ResponseDTO> UploadInvationTeamplateBackgroundImg(Guid courseVersionId, UploadInvationTeamplateBackgroundImg uploadCourseVersionBackgroundImg);
    Task<ResponseDTO> GetInvationTeamplateBackgrounds( Guid TemplateId);
}