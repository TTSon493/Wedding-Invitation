using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using AutoMapper;
using Wedding.Model.Domain;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Microsoft.Extensions.Configuration;
using Wedding.DataAccess.IRepository;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Wedding.Utility.Constants;

namespace Wedding.Service.Service;

public class InvitationTemplateService : IInvitationTemplateService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFirebaseService _firebaseService;

    public InvitationTemplateService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IFirebaseService firebaseService)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _firebaseService = firebaseService;
    }
    public async Task<ResponseDTO> GetAll(string? filterOn, string? filterQuery, string? sortBy, bool? isAscending,
        int pageNumber, int pageSize)
    {
        #region MyRegion

        try
        {
            List<InvitationTemplate> invitationTemplates = new List<InvitationTemplate>();
            
            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "templatename":
                    {
                        invitationTemplates = _unitOfWork.InvitationTemplateRepository.GetAllAsync()
                            .GetAwaiter().GetResult().Where(x => 
                                x.TemplateName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                    case "textcolor":
                    {
                        invitationTemplates = _unitOfWork.InvitationTemplateRepository.GetAllAsync()
                            .GetAwaiter().GetResult().Where(x => 
                                x.TextColor.Any(color => color.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase))).ToList();
                        break;
                    }
                    case "textfont":
                    {
                        invitationTemplates = _unitOfWork.InvitationTemplateRepository.GetAllAsync()
                            .GetAwaiter().GetResult().Where(x => 
                                x.TextFont.Any(font => font.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase))).ToList();
                        break;
                    }
                }
            }
            else
            {
                invitationTemplates = _unitOfWork.InvitationTemplateRepository.GetAllAsync()
                    .GetAwaiter().GetResult().ToList();
            }
            
            // Sort Query
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "templatename":
                    {
                        invitationTemplates = isAscending ==true
                            ? invitationTemplates.OrderBy(x => x.TemplateName).ToList()
                            : invitationTemplates.OrderByDescending(x => x.TemplateName).ToList();
                        break;
                    }
                    case "textcolor":
                    {
                        invitationTemplates = isAscending ==true
                            ? invitationTemplates.OrderBy(x => x.TextColor).ToList()
                            : invitationTemplates.OrderByDescending(x => x.TextColor).ToList();
                        break;
                    }
                    case "textfont":
                    {
                        invitationTemplates = isAscending ==true
                            ? invitationTemplates.OrderBy(x => x.TextFont).ToList()
                            : invitationTemplates.OrderByDescending(x => x.TextFont).ToList();
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
                invitationTemplates = invitationTemplates.Skip(skipResult).Take(pageSize).ToList();
            }
            
            #endregion Query Parameters

            if (invitationTemplates == null || !invitationTemplates.Any())
            {
                return new ResponseDTO()
                {
                    Message = "No invitations template found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }
            
            var invitationTemplateDtoList = new List<InvitationTemplateDTO>();

            foreach (var invitationTemplate in invitationTemplates)
            {
                var invitationTemplateDTO = new InvitationTemplateDTO
                {
                    TemplateId = invitationTemplate.TemplateId,
                    TemplateName = invitationTemplate.TemplateName,
                    BackgroundImageUrl = invitationTemplate.BackgroundImageUrl,
                    TextColor = invitationTemplate.TextColor,
                    TextFont = invitationTemplate.TextFont,
                    Description = invitationTemplate.Description,
                    CreatedBy = invitationTemplate.CreatedBy,
                    InvitationId = invitationTemplate.InvitationId
                };

                invitationTemplateDtoList.Add(invitationTemplateDTO);
            }

            return new ResponseDTO()
            {
                Message = "Get all invitations template successfully",
                Result = invitationTemplateDtoList,
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
                var invitationTemplate = await _unitOfWork.InvitationTemplateRepository.GetById(id);
                if (invitationTemplate is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Invitation template was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                InvitationTemplateDTO invitationTemplateDto = new InvitationTemplateDTO()
                {
                    TemplateId = invitationTemplate.TemplateId,
                    TemplateName = invitationTemplate.TemplateName,
                    BackgroundImageUrl = invitationTemplate.BackgroundImageUrl,
                    TextColor = invitationTemplate.TextColor,
                    TextFont = invitationTemplate.TextFont,
                    Description = invitationTemplate.Description,
                    CreatedBy = invitationTemplate.CreatedBy,
                    InvitationId = invitationTemplate.InvitationId
                };

                return new ResponseDTO()
                {
                    Message = "Get invitation template successfully ",
                    IsSuccess = false,
                    StatusCode = 200,
                    Result = invitationTemplateDto
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

    public async Task<ResponseDTO> UpdateById(Guid id, UpdateInvitationTemplateDTO updateInvitationTemplateDTO)
    {
        try
        {
            var invitationTemplateToUpdate = await _unitOfWork.InvitationTemplateRepository.GetById(id);
            if (invitationTemplateToUpdate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Invitation template was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }
            
            invitationTemplateToUpdate.TemplateName = updateInvitationTemplateDTO.TemplateName;
            invitationTemplateToUpdate.BackgroundImageUrl = updateInvitationTemplateDTO.BackgroundImageUrl;
            invitationTemplateToUpdate.TextColor = updateInvitationTemplateDTO.TextColor;
            invitationTemplateToUpdate.TextFont = updateInvitationTemplateDTO.TextFont;
            invitationTemplateToUpdate.Description = updateInvitationTemplateDTO.Description;
            invitationTemplateToUpdate.CreatedBy = updateInvitationTemplateDTO.CreatedBy;
            
            _unitOfWork.InvitationTemplateRepository.Update(invitationTemplateToUpdate);
            await _unitOfWork.SaveAsync();
            
            return new ResponseDTO()
            {
                Message = "Invitation template updated successfully",
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
            var invitationTemplate = await _unitOfWork.InvitationTemplateRepository.GetById(id);
            if (invitationTemplate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Invitation template was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            _unitOfWork.InvitationTemplateRepository.Remove(invitationTemplate);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Invitation template deleted successfully",
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

    public async Task<ResponseDTO> CreateById(CreateInvitationTemplateDTO createInvitationTemplateDTO)
    {
        try
        {
            var invitationTemplate = new InvitationTemplate()
            {
                TemplateId = Guid.NewGuid(),
                TemplateName = createInvitationTemplateDTO.TemplateName,
                BackgroundImageUrl = createInvitationTemplateDTO.BackgroundImageUrl,
                TextColor = createInvitationTemplateDTO.TextColor,
                TextFont = createInvitationTemplateDTO.TextFont,
                Description = createInvitationTemplateDTO.Description,
                CreatedBy = createInvitationTemplateDTO.CreatedBy,
                InvitationId = createInvitationTemplateDTO.InvitationId
            };

            await _unitOfWork.InvitationTemplateRepository.AddAsync(invitationTemplate);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Invitation template created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = invitationTemplate
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

    public async Task<ResponseDTO> UploadInvationTeamplateBackgroundImg(Guid TemplateId, UploadInvationTeamplateBackgroundImg uploadInvationTeamplateBackgroundImg)
    {
        try
        {
            if (uploadInvationTeamplateBackgroundImg.File == null)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "No file uploaded."
                };
            }

            var invationTemplate = await _unitOfWork.InvitationTemplateRepository.GetAsync(x => x.TemplateId == TemplateId);
            if (invationTemplate == null)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Invation template not found."
                };
            }
            
            var responseList = new List<string>();
            foreach (var image in uploadInvationTeamplateBackgroundImg.File)
            {
                var filePath = $"{StaticFirebaseFolders.InvitationTemplate}/{invationTemplate.TemplateId}/Background";
                var responseDto = await _firebaseService.UploadImage(image, filePath);
                if (responseDto.IsSuccess)
                {
                    responseList.Add(responseDto.Result.ToString());
                }
                else
                {
                    responseList.Add($"Failed to upload {image.FileName}: {responseDto.Message}");
                }
            }

            invationTemplate.BackgroundImageUrl = responseList.ToArray();;

            _unitOfWork.InvitationTemplateRepository.Update(invationTemplate);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                IsSuccess = true,
                StatusCode = 200,
                Result = responseList,
                Message = "Upload file successfully"
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO()
            {
                IsSuccess = false,
                StatusCode = 500,
                Result = null,
                Message = e.Message
            };
        }
    }

    public async Task<ResponseDTO> GetInvationTeamplateBackgrounds(Guid TemplateId)
    {
        try
        {
            var invationTemplate = await _unitOfWork.InvitationTemplateRepository.GetAsync(x => x.TemplateId == TemplateId);

            if (invationTemplate != null && invationTemplate.BackgroundImageUrl.IsNullOrEmpty())
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null,
                    Message = "No background image found"
                };
            }

            return new ResponseDTO()
            {
                IsSuccess = true,
                StatusCode = 200,
                Result = invationTemplate.BackgroundImageUrl,
                Message = "Get background images successfully"
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO()
            {
                IsSuccess = false,
                StatusCode = 500,
                Result = null,
                Message = "Internal server error"
            };
        }   
    }
}