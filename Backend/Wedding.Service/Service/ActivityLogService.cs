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

public class ActivityLogService : IActivityLogService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    
    public ActivityLogService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
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
            List<ActivityLog> activityLogs = new List<ActivityLog>();
            
            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "name":
                    {
                        activityLogs = _unitOfWork.ActivityLogRepository.GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().Where(x => 
                                x.ApplicationUser.FullName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                    case "action":
                    {
                        activityLogs = _unitOfWork.ActivityLogRepository.GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().Where(x => 
                                x.Action.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                    case "entity":
                    {
                        activityLogs = _unitOfWork.ActivityLogRepository.GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().Where(x => 
                                x.Entity.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                    
                    case "timestamp":
                    {
                        activityLogs = _unitOfWork.ActivityLogRepository.GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().Where(x => 
                                x.TimeStamp.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                    default:
                    {
                        activityLogs = _unitOfWork.ActivityLogRepository
                            .GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().ToList();
                        break;
                    }
                }
            }
            else
            {
                activityLogs = _unitOfWork.ActivityLogRepository.GetAllAsync(includeProperties: "ApplicationUser")
                    .GetAwaiter().GetResult().ToList();
            }
            
            // Sort Query
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "name":
                    {
                        activityLogs = isAscending ==true
                            ? activityLogs.OrderBy(x => x.ApplicationUser.FullName).ToList()
                            : activityLogs.OrderByDescending(x => x.ApplicationUser.FullName).ToList();
                        break;
                    }
                    case "action":
                    {
                        activityLogs = isAscending ==true
                            ? activityLogs.OrderBy(x => x.Action).ToList()
                            : activityLogs.OrderByDescending(x => x.Action).ToList();
                        break;
                    }
                    case "entity":
                    {
                        activityLogs = isAscending ==true
                            ? activityLogs.OrderBy(x => x.Entity).ToList()
                            : activityLogs.OrderByDescending(x => x.Entity).ToList();
                        break;
                    }
                    case "timestamp":
                    {
                        activityLogs = isAscending ==true
                            ? activityLogs.OrderBy(x => x.TimeStamp).ToList()
                            : activityLogs.OrderByDescending(x => x.TimeStamp).ToList();
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
                activityLogs = activityLogs.Skip(skipResult).Take(pageSize).ToList();
            }
            
            #endregion Query Parameters

            if (activityLogs == null || !activityLogs.Any())
            {
                return new ResponseDTO()
                {
                    Message = "No activity logs found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }
            
            var activityLogDtoList = new List<ActivityLogDTO>();

            foreach (var activityLog in activityLogs)
            {
                var activityLogDTO = new ActivityLogDTO
                {
                    LogId = activityLog.LogId,
                    UserId = activityLog.ApplicationUser?.FullName,
                    Action = activityLog.Action,
                    Entity = activityLog.Entity,
                    TimeStamp = activityLog.TimeStamp,
                };

                activityLogDtoList.Add(activityLogDTO);
            }

            return new ResponseDTO()
            {
                Message = "Get all activity logs successfully",
                Result = activityLogDtoList,
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

    public async Task<ResponseDTO> UpdateById(UpdateActivityLogDTO updateActivityLogDTO)
    {
        try
        {
            var activityLogToUpdate = await _unitOfWork.ActivityLogRepository.GetById(updateActivityLogDTO.LogId);
            if (activityLogToUpdate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Activity log was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }
            
            activityLogToUpdate.Action = updateActivityLogDTO.Action;
            activityLogToUpdate.Entity = updateActivityLogDTO.Entity;
            activityLogToUpdate.TimeStamp = updateActivityLogDTO.TimeStamp;
            
            _unitOfWork.ActivityLogRepository.Update(activityLogToUpdate);
            await _unitOfWork.SaveAsync();
            
            return new ResponseDTO()
            {
                Message = "Activity log updated successfully",
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

            _unitOfWork.ActivityLogRepository.Remove(activityLog);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Activity log deleted successfully",
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

    public async Task<ResponseDTO> CreateById(CreateActivityLogDTO createActivityLogDTO)
    {
        try
        {
            var activityLog = new ActivityLog
            {
                LogId = Guid.NewGuid(),
                UserId = createActivityLogDTO.UserId,
                Action = createActivityLogDTO.Action,
                Entity = createActivityLogDTO.Entity,
                TimeStamp = createActivityLogDTO.TimeStamp
            };

            await _unitOfWork.ActivityLogRepository.AddAsync(activityLog);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Activity log created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = activityLog
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