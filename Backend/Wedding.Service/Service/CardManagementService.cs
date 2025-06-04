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

namespace Wedding.Service.Service;

public class CardManagementService : ICardManagementService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    
    public CardManagementService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseDTO> GetAll(ClaimsPrincipal User, string? filterOn, string? filterQuery, string? sortBy, bool? isAscending,
        int pageNumber, int pageSize)
    {
        #region MyRegion

        try
        {
            List<CardManagement> cardManagements = new List<CardManagement>();
            
            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "name":
                    {
                        cardManagements = _unitOfWork.CardManagementRepository.GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().Where(x => 
                                x.ApplicationUser.FullName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                    case "attendstatus":
                    {
                        cardManagements = _unitOfWork.CardManagementRepository.GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().Where(x => 
                                x.AttendStatus.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                    default:
                    {
                        cardManagements = _unitOfWork.CardManagementRepository
                            .GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().ToList();
                        break;
                    }
                }
            }
            else
            {
                cardManagements = _unitOfWork.CardManagementRepository.GetAllAsync(includeProperties: "ApplicationUser")
                    .GetAwaiter().GetResult().ToList();
            }
            
            // Sort Query
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "name":
                    {
                        cardManagements = isAscending ==true
                            ? cardManagements.OrderBy(x => x.ApplicationUser.FullName).ToList()
                            : cardManagements.OrderByDescending(x => x.ApplicationUser.FullName).ToList();
                        break;
                    }
                    case "attendstatus":
                    {
                        cardManagements = isAscending ==true
                            ? cardManagements.OrderBy(x => x.AttendStatus).ToList()
                            : cardManagements.OrderByDescending(x => x.AttendStatus).ToList();
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
                cardManagements = cardManagements.Skip(skipResult).Take(pageSize).ToList();
            }
            
            #endregion Query Parameters

            if (cardManagements == null || !cardManagements.Any())
            {
                return new ResponseDTO()
                {
                    Message = "No card managements found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }
            
            var cardManagementDtoList = new List<CardManagementDTO>();

            foreach (var cardManagement in cardManagements)
            {
                var cardManagementDTO = new CardManagementDTO
                {
                    CardId = cardManagement.CardId,
                    UserId = cardManagement.ApplicationUser?.FullName,
                    InvitationId = cardManagement.InvitationId,
                    AttendStatus = cardManagement.AttendStatus,
                    CreatedTime = cardManagement.CreatedTime,
                };

                cardManagementDtoList.Add(cardManagementDTO);
            }

            return new ResponseDTO()
            {
                Message = "Get all cards management successfully",
                Result = cardManagementDtoList,
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
                var activityLog = await _unitOfWork.ActivityLogRepository.GetById(id);
                if (activityLog is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Activity log was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                ActivityLogDTO activityLogDto = new ActivityLogDTO()
                {
                    LogId = activityLog.LogId,
                    UserId = activityLog.UserId,
                    Action = activityLog.Action,
                    Entity = activityLog.Entity,
                    TimeStamp = activityLog.TimeStamp
                };

                return new ResponseDTO()
                {
                    Message = "Get activity log successfully ",
                    IsSuccess = false,
                    StatusCode = 200,
                    Result = activityLogDto
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

    public async Task<ResponseDTO> UpdateById(UpdateCardManagementDTO updateCardManagementDTO)
    {
        try
        {
            var cardManagementToUpdate = await _unitOfWork.CardManagementRepository.GetById(updateCardManagementDTO.CardId);
            if (cardManagementToUpdate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Card management was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }
            
            cardManagementToUpdate.AttendStatus = updateCardManagementDTO.AttendStatus;
            cardManagementToUpdate.CreatedTime = updateCardManagementDTO.CreatedTime;
            
            _unitOfWork.CardManagementRepository.Update(cardManagementToUpdate);
            await _unitOfWork.SaveAsync();
            
            return new ResponseDTO()
            {
                Message = "Card management updated successfully",
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
            var cardManagement = await _unitOfWork.CardManagementRepository.GetById(id);
            if (cardManagement is null)
            {
                return new ResponseDTO()
                {
                    Message = "Card management was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            _unitOfWork.CardManagementRepository.Remove(cardManagement);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Card management deleted successfully",
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

    public async Task<ResponseDTO> CreateById(CreateCardManagementDTO cardManagementDTO)
    {
        try
        {
            var cardManagement = new CardManagement()
            {
                CardId = Guid.NewGuid(),
                UserId = cardManagementDTO.UserId,
                InvitationId = cardManagementDTO.InvitationId,
                AttendStatus = cardManagementDTO.AttendStatus,
                CreatedTime = cardManagementDTO.CreatedTime
            };

            await _unitOfWork.CardManagementRepository.AddAsync(cardManagement);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Card management created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = cardManagement
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