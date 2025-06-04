using Wedding.Model.Domain;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Wedding.DataAccess.IRepository;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Wedding.Service.Service;

public class InvitationHtmlService : IInvitationHtmlService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    
    public InvitationHtmlService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDTO> GetAll(string? filterOn, string? filterQuery, string? sortBy, bool? isAscending,
        int pageNumber, int pageSize)
    {
        #region MyRegion

        try
        {
            List<InvitationHtml> invitationHtmls = new List<InvitationHtml>();
            
            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "htmlcontent":
                    {
                        invitationHtmls = _unitOfWork.InvitationHtmlRepository.GetAllAsync()
                            .GetAwaiter().GetResult().Where(x => 
                                x.HtmlContent.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                }
            }
            else
            {
                invitationHtmls = _unitOfWork.InvitationHtmlRepository.GetAllAsync()
                    .GetAwaiter().GetResult().ToList();
            }
            
            // Sort Query
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "htmlcontent":
                    {
                        invitationHtmls = isAscending ==true
                            ? invitationHtmls.OrderBy(x => x.HtmlContent).ToList()
                            : invitationHtmls.OrderByDescending(x => x.HtmlContent).ToList();
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
            
            // Pagination
            if (pageNumber > 0 && pageSize > 0)
            {
                var skipResult = (pageNumber - 1) * pageSize;
                invitationHtmls = invitationHtmls.Skip(skipResult).Take(pageSize).ToList();
            }
            
            #endregion Query Parameters

            if (invitationHtmls == null || !invitationHtmls.Any())
            {
                return new ResponseDTO()
                {
                    Message = "No invitation html found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }
            
            var invitationHtmlDtoList = new List<InvitationHtmlDTO>();

            foreach (var invitationHtml in invitationHtmls)
            {
                var invitationHtmlDTO = new InvitationHtmlDTO
                {
                    HtmlId = invitationHtml.HtmlId,
                    InvitationId = invitationHtml.InvitationId,
                    HtmlContent = invitationHtml.HtmlContent,
                    CreatedTime = invitationHtml.CreatedTime,
                    UpdateddTime = invitationHtml.UpdateddTime,
                };

                invitationHtmlDtoList.Add(invitationHtmlDTO);
            }

            return new ResponseDTO()
            {
                Message = "Get all invitation html successfully",
                Result = invitationHtmlDtoList,
                IsSuccess = true,
                StatusCode = 200
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO()
            {
                Message = e.Message,
                Result = null,
                IsSuccess = false,
                StatusCode = 500
            };
        }
    }

    public async Task<ResponseDTO> GetById(Guid id)
    {
        {
            try
            {
                var invitationHtml = await _unitOfWork.InvitationHtmlRepository.GetById(id);
                if (invitationHtml is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Invitation html was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                InvitationHtmlDTO invitationHtmlDto = new InvitationHtmlDTO()
                {
                    HtmlId = invitationHtml.HtmlId,
                    InvitationId = invitationHtml.InvitationId,
                    HtmlContent = invitationHtml.HtmlContent,
                    CreatedTime = invitationHtml.CreatedTime,
                    UpdateddTime = invitationHtml.UpdateddTime,
                };

                return new ResponseDTO()
                {
                    Message = "Get invitation html successfully ",
                    IsSuccess = false,
                    StatusCode = 200,
                    Result = invitationHtmlDto
                };
            }
            catch (Exception e)
            {
                return new ResponseDTO()
                {
                    Message = e.Message,
                    IsSuccess = false,
                    StatusCode = 500,
                    Result = null
                };
            }
        }
    }

    public async Task<ResponseDTO> UpdateById(Guid id, UpdateInvitationHtmlDTO updateInvitationHtmlDTO)
    {
        try
        {
            var invitationHtmlToUpdate = await _unitOfWork.InvitationHtmlRepository.GetById(id);
            if (invitationHtmlToUpdate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Invitation html was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            invitationHtmlToUpdate.HtmlContent = updateInvitationHtmlDTO.HtmlContent;
            invitationHtmlToUpdate.CreatedTime = updateInvitationHtmlDTO.CreatedTime;
            invitationHtmlToUpdate.UpdateddTime = updateInvitationHtmlDTO.UpdateddTime;
            
            _unitOfWork.InvitationHtmlRepository.Update(invitationHtmlToUpdate);
            await _unitOfWork.SaveAsync();
            
            return new ResponseDTO()
            {
                Message = "Invitation html updated successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = null
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = null
            };
        }
    }

    public async Task<ResponseDTO> DeleteById(Guid id)
    {
        try
        {
            var invitationHtml = await _unitOfWork.InvitationHtmlRepository.GetById(id);
            if (invitationHtml is null)
            {
                return new ResponseDTO()
                {
                    Message = "Invitation html was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            _unitOfWork.InvitationHtmlRepository.Remove(invitationHtml);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Invitation html deleted successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = null
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = null
            };
        }
    }

    public async Task<ResponseDTO> CreateById(CreateInvitationHtmlDTO createInvitationHtmlDTO)
    {
        try
        {
            var invitationHtml = new InvitationHtml()
            {
                HtmlId = Guid.NewGuid(),
                InvitationId = createInvitationHtmlDTO.InvitationId,
                HtmlContent = createInvitationHtmlDTO.HtmlContent,
                CreatedTime = createInvitationHtmlDTO.CreatedTime,
                UpdateddTime = createInvitationHtmlDTO.UpdateddTime,
            };

            await _unitOfWork.InvitationHtmlRepository.AddAsync(invitationHtml);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Invitation html created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = invitationHtml
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = null
            };
        }
    }
}