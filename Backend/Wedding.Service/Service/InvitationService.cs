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
using Wedding.Utility.Constants;

namespace Wedding.Service.Service;

public class InvitationService : IInvitationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFirebaseService _firebaseService;
    
    public InvitationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IFirebaseService firebaseService)
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
            List<Invitation> invitations = new List<Invitation>();
            
            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "customermessage":
                    {
                        invitations = _unitOfWork.InvitationRepository.GetAllAsync()
                            .GetAwaiter().GetResult().Where(x => 
                                x.CustomerMessage.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                    case "customertextcolor":
                    {
                        invitations = _unitOfWork.InvitationRepository.GetAllAsync()
                            .GetAwaiter().GetResult().Where(x => 
                                x.CustomerTextColor.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                }
            }
            else
            {
                invitations = _unitOfWork.InvitationRepository.GetAllAsync()
                    .GetAwaiter().GetResult().ToList();
            }
            
            // Sort Query
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "customermessage":
                    {
                        invitations = isAscending ==true
                            ? invitations.OrderBy(x => x.CustomerMessage).ToList()
                            : invitations.OrderByDescending(x => x.CustomerMessage).ToList();
                        break;
                    }
                    case "customertextcolor":
                    {
                        invitations = isAscending ==true
                            ? invitations.OrderBy(x => x.CustomerTextColor).ToList()
                            : invitations.OrderByDescending(x => x.CustomerTextColor).ToList();
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
                invitations = invitations.Skip(skipResult).Take(pageSize).ToList();
            }
            
            #endregion Query Parameters

            if (invitations == null || !invitations.Any())
            {
                return new ResponseDTO()
                {
                    Message = "No invitations found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }
            
            var invitationDtoList = new List<InvitationDTO>();

            foreach (var invitation in invitations)
            {
                var invitationDTO = new InvitationDTO
                {
                    InvitationId = invitation.InvitationId,
                    WeddingId = invitation.WeddingId,
                    TemplateId = invitation.TemplateId,
                    CustomerMessage = invitation.CustomerMessage,
                    CustomerTextColor = invitation.CustomerTextColor,
                    ShareableLink = invitation.ShareableLink,
                    CreatedTime = invitation.CreatedTime,
                };

                invitationDtoList.Add(invitationDTO);
            }

            return new ResponseDTO()
            {
                Message = "Get all invitations successfully",
                Result = invitationDtoList,
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
                var invitation = await _unitOfWork.InvitationRepository.GetById(id);
                if (invitation is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Invitation was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                InvitationDTO invitationDto = new InvitationDTO()
                {
                    InvitationId = invitation.InvitationId,
                    WeddingId = invitation.WeddingId,
                    TemplateId = invitation.TemplateId,
                    CustomerMessage = invitation.CustomerMessage,
                    CustomerTextColor = invitation.CustomerTextColor,
                    ShareableLink = invitation.ShareableLink,
                    CreatedTime = invitation.CreatedTime
                };

                return new ResponseDTO()
                {
                    Message = "Get invitation successfully ",
                    IsSuccess = false,
                    StatusCode = 200,
                    Result = invitationDto
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

    public async Task<ResponseDTO> UpdateById(Guid id, UpdateInvitationDTO updateInvitationDTO)
    {
        try
        {
            var invitationToUpdate = await _unitOfWork.InvitationRepository.GetById(id);
            if (invitationToUpdate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Invitatio was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            invitationToUpdate.InvationLocation = updateInvitationDTO.InvationLocation;
            invitationToUpdate.InvitationPhotoUrl = updateInvitationDTO.InvitationPhotoUrl;
            invitationToUpdate.CustomerMessage = updateInvitationDTO.CustomerMessage;
            invitationToUpdate.CustomerTextColor = updateInvitationDTO.CustomerTextColor;
            invitationToUpdate.CreatedTime = updateInvitationDTO.CreatedTime;
            
            _unitOfWork.InvitationRepository.Update(invitationToUpdate);
            await _unitOfWork.SaveAsync();
            
            return new ResponseDTO()
            {
                Message = "Invitation updated successfully",
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
            var invitation = await _unitOfWork.InvitationRepository.GetById(id);
            if (invitation is null)
            {
                return new ResponseDTO()
                {
                    Message = "Invitation was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            _unitOfWork.InvitationRepository.Remove(invitation);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Invitation deleted successfully",
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

    public async Task<ResponseDTO> CreateById(CreateInvitationDTO createInvitationDTO)
    {
        try
        {
            var invitation = new Invitation()
            {
                InvitationId = Guid.NewGuid(),
                TemplateId = createInvitationDTO.TemplateId,
                WeddingId = createInvitationDTO.WeddingId,
                InvationLocation = createInvitationDTO.InvationLocation,
                InvitationPhotoUrl = createInvitationDTO.InvitationPhotoUrl,
                CustomerMessage = createInvitationDTO.CustomerMessage,
                CustomerTextColor = createInvitationDTO.CustomerTextColor,
                ShareableLink = createInvitationDTO.ShareableLink,
                CreatedTime = createInvitationDTO.CreatedTime
            };

            await _unitOfWork.InvitationRepository.AddAsync(invitation);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Invitation created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = invitation
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
    
    public async Task<ResponseDTO> UploadInvationBackground(Guid InvationId, UploadInvationBackgroundImg uploadInvationBackgroundImg)
    {
        try
        {
            if (uploadInvationBackgroundImg.File == null)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "No file uploaded."
                };
            }

            var invation = await _unitOfWork.InvitationRepository.GetAsync(x => x.InvitationId == InvationId);
            if (invation == null)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Invation not found."
                };
            }
            
            var responseList = new List<string>();
            foreach (var image in uploadInvationBackgroundImg.File)
            {
                var filePath = $"{StaticFirebaseFolders.EventPhoto}/{invation.InvitationId}/Background";
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

            invation.InvitationPhotoUrl = responseList.ToArray();;
            
            _unitOfWork.InvitationRepository.Update(invation);
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

    public async Task<ResponseDTO> GetInvationBackground(Guid InvationId)
    {
        try
        {
            var invation = await _unitOfWork.InvitationRepository.GetAsync(x => x.InvitationId == InvationId);

            if (invation != null && invation.InvitationPhotoUrl.IsNullOrEmpty())
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
                Result = invation.InvitationPhotoUrl,
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