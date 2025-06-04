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

public class GuestService : IGuestService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public GuestService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
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
            List<Guest> Guests = new List<Guest>();

            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "name":
                        {
                            Guests = _unitOfWork.GuestRepository.GetAllAsync()
                                .GetAwaiter().GetResult().Where(x =>
                                    x.Name.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            break;
                        }
                    case "attend":
                        {
                            Guests = _unitOfWork.GuestRepository.GetAllAsync()
                                .GetAwaiter().GetResult().Where(x =>
                                    x.Attend.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            break;
                        }
                    case "gift":
                        {
                            Guests = _unitOfWork.GuestRepository.GetAllAsync()
                                .GetAwaiter().GetResult().Where(x =>
                                    x.Gift.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            break;
                        }
                    default:
                        {
                            Guests = _unitOfWork.GuestRepository
                                .GetAllAsync()
                                .GetAwaiter().GetResult().ToList();
                            break;
                        }
                }
            }
            else
            {
                Guests = _unitOfWork.GuestRepository.GetAllAsync()
                    .GetAwaiter().GetResult().ToList();
            }

            // Sort Query
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "name":
                        {
                            Guests = isAscending == true
                                ? Guests.OrderBy(x => x.Name).ToList()
                                : Guests.OrderByDescending(x => x.Name).ToList();
                            break;
                        }
                    case "attend":
                        {
                            Guests = isAscending == true
                                ? Guests.OrderBy(x => x.Attend).ToList()
                                : Guests.OrderByDescending(x => x.Attend).ToList();
                            break;
                        }
                    case "gift":
                        {
                            Guests = isAscending == true
                                ? Guests.OrderBy(x => x.Gift).ToList()
                                : Guests.OrderByDescending(x => x.Gift).ToList();
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
                Guests = Guests.Skip(skipResult).Take(pageSize).ToList();
            }

            #endregion Query Parameters

            if (Guests == null || !Guests.Any())
            {
                return new ResponseDTO()
                {
                    Message = "No Guest found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var GuestDtoList = new List<GuestDTO>();

            foreach (var Guest in Guests)
            {
                var GuestDTO = new GuestDTO
                {
                    GuestId = Guest.GuestId,
                    EventId = Guest.EventId,
                    Name = Guest.Name,
                    Attend = Guest.Attend,
                    Gift = Guest.Gift,
                };

                GuestDtoList.Add(GuestDTO);
            }

            return new ResponseDTO()
            {
                Message = "Get all Guest successfully",
                Result = GuestDtoList,
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
                var Guest = await _unitOfWork.GuestRepository.GetById(id);
                if (Guest is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Guest was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                GuestDTO GuestDto = new GuestDTO()
                {
                    GuestId = Guest.GuestId,
                    EventId = Guest.EventId,
                    Name = Guest.Name,
                    Attend = Guest.Attend,
                    Gift = Guest.Gift,
                };

                return new ResponseDTO()
                {
                    Message = "Get Guest successfully ",
                    IsSuccess = false,
                    StatusCode = 200,
                    Result = GuestDto
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

    public async Task<ResponseDTO> UpdateById(Guid id, UpdateGuestDTO updateGuestDTO)
    {
        try
        {
            var GuestToUpdate = await _unitOfWork.GuestRepository.GetById(id);
            if (GuestToUpdate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Guest was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            GuestToUpdate.Name = updateGuestDTO.Name;
            GuestToUpdate.Attend = updateGuestDTO.Attend;
            GuestToUpdate.Gift = updateGuestDTO.Gift;

            _unitOfWork.GuestRepository.Update(GuestToUpdate);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Guest updated successfully",
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
            var Guest = await _unitOfWork.GuestRepository.GetById(id);
            if (Guest is null)
            {
                return new ResponseDTO()
                {
                    Message = "Guest was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            _unitOfWork.GuestRepository.Remove(Guest);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Guest deleted successfully",
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

    public async Task<ResponseDTO> CreateById(CreateGuestDTO createGuestDTO)
    {
        try
        {
            var Guest = new Guest
            {
                GuestId = Guid.NewGuid(),
                GuestListId = createGuestDTO.GuestListId,
                EventId = createGuestDTO.EventId,
                Name = createGuestDTO.Name,
                Attend = createGuestDTO.Attend,
                Gift = createGuestDTO.Gift,
            };

            await _unitOfWork.GuestRepository.AddAsync(Guest);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Guest created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = Guest
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