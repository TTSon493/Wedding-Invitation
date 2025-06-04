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
using Microsoft.Extensions.DependencyInjection;
using Wedding.Utility.Constants;

namespace Wedding.Service.Service;

public class EventPhotoService : IEventPhotoService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFirebaseService _firebaseService;

    public EventPhotoService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
        IFirebaseService firebaseService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _firebaseService = firebaseService;
    }

    public async Task<ResponseDTO> GetAll(string? filterOn, string? filterQuery, string? sortBy, bool? isAscending,
        int pageNumber, int pageSize)
    {
        #region MyRegion

        try
        {
            List<EventPhoto> EventPhotos = new List<EventPhoto>();

            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "phototype":
                    {
                        EventPhotos = _unitOfWork.EventPhotoRepository.GetAllAsync()
                            .GetAwaiter().GetResult().Where(x =>
                                x.PhotoType.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                    default:
                    {
                        EventPhotos = _unitOfWork.EventPhotoRepository
                            .GetAllAsync()
                            .GetAwaiter().GetResult().ToList();
                        break;
                    }
                }
            }
            else
            {
                EventPhotos = _unitOfWork.EventPhotoRepository.GetAllAsync()
                    .GetAwaiter().GetResult().ToList();
            }

            // Sort Query
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "phototype":
                    {
                        EventPhotos = isAscending == true
                            ? EventPhotos.OrderBy(x => x.PhotoType).ToList()
                            : EventPhotos.OrderByDescending(x => x.PhotoType).ToList();
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
                EventPhotos = EventPhotos.Skip(skipResult).Take(pageSize).ToList();
            }

            #endregion Query Parameters

            if (EventPhotos == null || !EventPhotos.Any())
            {
                return new ResponseDTO()
                {
                    Message = "No Photos found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var EventPhotoDtoList = new List<EventPhotoDTO>();

            foreach (var EventPhoto in EventPhotos)
            {
                var EventPhotoDTO = new EventPhotoDTO
                {
                    EventPhotoId = EventPhoto.EventPhotoId,
                    PhotoUrl = EventPhoto.PhotoUrl,
                    PhotoType = EventPhoto.PhotoType,
                    UploadedDate = EventPhoto.UploadedDate,
                };

                EventPhotoDtoList.Add(EventPhotoDTO);
            }

            return new ResponseDTO()
            {
                Message = "Get all Photos successfully",
                Result = EventPhotoDtoList,
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
                var EventPhoto = await _unitOfWork.EventPhotoRepository.GetById(id);
                if (EventPhoto is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Photo was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                EventPhotoDTO EventPhotoDto = new EventPhotoDTO()
                {
                    EventPhotoId = EventPhoto.EventPhotoId,
                    PhotoUrl = EventPhoto.PhotoUrl,
                    PhotoType = EventPhoto.PhotoType,
                    UploadedDate = EventPhoto.UploadedDate,
                };

                return new ResponseDTO()
                {
                    Message = "Get Photo successfully ",
                    IsSuccess = false,
                    StatusCode = 200,
                    Result = EventPhotoDto
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

    public async Task<ResponseDTO> UpdateById(Guid id, UpdateEventPhotoDTO updateEventPhotoDTO)
    {
        try
        {
            var EventPhotoToUpdate = await _unitOfWork.EventPhotoRepository.GetById(id);
            if (EventPhotoToUpdate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Photo was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            EventPhotoToUpdate.PhotoUrl = updateEventPhotoDTO.PhotoUrl;
            EventPhotoToUpdate.PhotoType = updateEventPhotoDTO.PhotoType;
            EventPhotoToUpdate.UploadedDate = updateEventPhotoDTO.UploadedDate;

            _unitOfWork.EventPhotoRepository.Update(EventPhotoToUpdate);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Photo updated successfully",
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
            var EventPhoto = await _unitOfWork.EventPhotoRepository.GetById(id);
            if (EventPhoto is null)
            {
                return new ResponseDTO()
                {
                    Message = "Photo was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            _unitOfWork.EventPhotoRepository.Remove(EventPhoto);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Photo deleted successfully",
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

    public async Task<ResponseDTO> CreateById(CreateEventPhotoDTO createEventPhotoDTO)
    {
        try
        {
            var EventPhoto = new EventPhoto
            {
                EventPhotoId = Guid.NewGuid(),
                EventId = createEventPhotoDTO.EventId,
                PhotoUrl = createEventPhotoDTO.PhotoUrl,
                PhotoType = createEventPhotoDTO.PhotoType,
                UploadedDate = createEventPhotoDTO.UploadedDate,
            };

            await _unitOfWork.EventPhotoRepository.AddAsync(EventPhoto);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Photo created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = EventPhoto
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

    public async Task<ResponseDTO> UploadEventPhotoBackground(Guid EventPhotoId,
        UploadEventPhotoBackgroundImg uploadEventPhotoBackgroundImg)
    {
        try
        {
            if (uploadEventPhotoBackgroundImg.File == null)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "No file uploaded."
                };
            }

            var eventPhoto = await _unitOfWork.EventPhotoRepository.GetAsync(x => x.EventPhotoId == EventPhotoId);
            if (eventPhoto == null)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Event photo not found."
                };
            }
            
            var responseList = new List<string>();
            foreach (var image in uploadEventPhotoBackgroundImg.File)
            {
                var filePath = $"{StaticFirebaseFolders.EventPhoto}/{eventPhoto.EventPhotoId}/Background";
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

            eventPhoto.PhotoUrl = responseList.ToArray();;

            _unitOfWork.EventPhotoRepository.Update(eventPhoto);
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

    public async Task<ResponseDTO> GetEventPhotoBackground(Guid EventPhotoId)
    {
        try
        {
            var eventPhoto = await _unitOfWork.EventPhotoRepository.GetAsync(x => x.EventPhotoId == EventPhotoId);

            if (eventPhoto != null && eventPhoto.PhotoUrl.IsNullOrEmpty())
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
                Result = eventPhoto.PhotoUrl,
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