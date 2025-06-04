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
using Microsoft.Extensions.Logging;

namespace Wedding.Service.Service;

public class GuestListService : IGuestListService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public GuestListService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
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
            List<GuestList> GuestLists = new List<GuestList>();

            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {

                    case "guestname":
                        {
                            GuestLists = _unitOfWork.GuestListRepository.GetAllAsync()
                                .GetAwaiter().GetResult().Where(x =>
                                    x.GuestName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            break;
                        }
                    case "guestgift":
                        {
                            GuestLists = _unitOfWork.GuestListRepository.GetAllAsync()
                                .GetAwaiter().GetResult().Where(x =>
                                    x.GuestGift.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            break;
                        }
                    default:
                        {
                            GuestLists = _unitOfWork.GuestListRepository
                                .GetAllAsync()
                                .GetAwaiter().GetResult().ToList();
                            break;
                        }
                }
            }
            else
            {
                GuestLists = _unitOfWork.GuestListRepository.GetAllAsync()
                    .GetAwaiter().GetResult().ToList();
            }

            // Sort Query
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "guestname":
                        {
                            GuestLists = isAscending == true
                                ? GuestLists.OrderBy(x => x.GuestName).ToList()
                                : GuestLists.OrderByDescending(x => x.GuestName).ToList();
                            break;
                        }
                    case "guestgift":
                        {
                            GuestLists = isAscending == true
                                ? GuestLists.OrderBy(x => x.GuestGift).ToList()
                                : GuestLists.OrderByDescending(x => x.GuestGift).ToList();
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
                GuestLists = GuestLists.Skip(skipResult).Take(pageSize).ToList();
            }

            #endregion Query Parameters

            if (GuestLists == null || !GuestLists.Any())
            {
                return new ResponseDTO()
                {
                    Message = "No Guest listt found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var GuestListDtoList = new List<GuestListDTO>();

            foreach (var GuestList in GuestLists)
            {
                var GuestListDTO = new GuestListDTO
                {
                    
                    GuestListId = GuestList.GuestListId,
                    EventId = GuestList.EventId,
                    GuestName = GuestList.GuestName,
                    AttendStatus = GuestList.AttendStatus,
                    CheckinTime = GuestList.CheckinTime,
                    GuestGift = GuestList.GuestGift,
                };

                GuestListDtoList.Add(GuestListDTO);
            }

            return new ResponseDTO()
            {
                Message = "Get all Guest list successfully",
                Result = GuestListDtoList,
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
                var GuestList = await _unitOfWork.GuestListRepository.GetById(id);
                if (GuestList is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Guest list was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                GuestListDTO GuestListDto = new GuestListDTO()
                {
                    GuestListId = GuestList.GuestListId,
                    EventId = GuestList.EventId,
                    GuestName = GuestList.GuestName,
                    AttendStatus = GuestList.AttendStatus,
                    CheckinTime = GuestList.CheckinTime,
                    GuestGift = GuestList.GuestGift,
                };

                return new ResponseDTO()
                {
                    Message = "Get Guest list successfully ",
                    IsSuccess = false,
                    StatusCode = 200,
                    Result = GuestListDto
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

    public async Task<ResponseDTO> UpdateById(Guid id, UpdateGuestListDTO updateGuestListDTO)
    {
        try
        {
            var GuestListToUpdate = await _unitOfWork.GuestListRepository.GetById(id);
            if (GuestListToUpdate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Guest list was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            GuestListToUpdate.GuestName = updateGuestListDTO.GuestName;
            GuestListToUpdate.AttendStatus = updateGuestListDTO.AttendStatus;
            GuestListToUpdate.CheckinTime = updateGuestListDTO.CheckinTime;
            GuestListToUpdate.GuestGift = updateGuestListDTO.GuestGift;

            _unitOfWork.GuestListRepository.Update(GuestListToUpdate);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Guest list updated successfully",
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
            var GuestList = await _unitOfWork.GuestListRepository.GetById(id);
            if (GuestList is null)
            {
                return new ResponseDTO()
                {
                    Message = "Guest list was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            _unitOfWork.GuestListRepository.Remove(GuestList);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Guest list deleted successfully",
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

    public async Task<ResponseDTO> CreateById(CreateGuestListDTO createGuestListDTO)
    {
        try
        {
            var GuestList = new GuestList
            {
                GuestListId = Guid.NewGuid(),
                GuestId = createGuestListDTO.GuestId,
                EventId = createGuestListDTO.EventId,
                GuestName = createGuestListDTO.GuestName,
                AttendStatus = createGuestListDTO.AttendStatus,
                CheckinTime = createGuestListDTO.CheckinTime,
                GuestGift = createGuestListDTO.GuestGift,
            };

            await _unitOfWork.GuestListRepository.AddAsync(GuestList);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Guest list created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = GuestList
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