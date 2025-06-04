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

public class EventService : IEventService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFirebaseService _firebaseService;

    public EventService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IFirebaseService firebaseService)
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
            List<Event> Events = new List<Event>();

            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "bridgename":
                        {
                            Events = _unitOfWork.EventRepository.GetAllAsync()
                                .GetAwaiter().GetResult().Where(x =>
                                    x.BrideName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            break;
                        }
                    case "groomname":
                        {
                            Events = _unitOfWork.EventRepository.GetAllAsync()
                                .GetAwaiter().GetResult().Where(x =>
                                    x.GroomName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            break;
                        }

                    case "eventlocation":
                        {
                            Events = _unitOfWork.EventRepository.GetAllAsync()
                                .GetAwaiter().GetResult().Where(x =>
                                    x.EventLocation.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                            break;
                        }
                    default:
                        {
                            Events = _unitOfWork.EventRepository
                                .GetAllAsync()
                                .GetAwaiter().GetResult().ToList();
                            break;
                        }
                }
            }
            else
            {
                Events = _unitOfWork.EventRepository.GetAllAsync()
                    .GetAwaiter().GetResult().ToList();
            }

            // Sort Query
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "bridgename":
                        {
                            Events = isAscending == true
                                ? Events.OrderBy(x => x.BrideName).ToList()
                                : Events.OrderByDescending(x => x.BrideName).ToList();
                            break;
                        }
                    case "groomname":
                        {
                            Events = isAscending == true
                                ? Events.OrderBy(x => x.GroomName).ToList()
                                : Events.OrderByDescending(x => x.GroomName).ToList();
                            break;
                        }
                    case "eventlocation":
                        {
                            Events = isAscending == true
                                ? Events.OrderBy(x => x.EventLocation).ToList()
                                : Events.OrderByDescending(x => x.EventLocation).ToList();
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
                Events = Events.Skip(skipResult).Take(pageSize).ToList();
            }

            #endregion Query Parameters

            if (Events == null || !Events.Any())
            {
                return new ResponseDTO()
                {
                    Message = "No events found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var EventDtoList = new List<EventDTO>();

            foreach (var Event in Events)
            {
                var EventDTO = new EventDTO
                {
                    EventId = Event.EventId,
                    WeddingId = Event.WeddingId,
                    BrideName = Event.BrideName,
                    GroomName = Event.GroomName,
                    EventDate = Event.EventDate,
                    EventLocation = Event.EventLocation,
                    EventPhotoUrl = Event.EventPhotoUrl,
                    CreatedDate = Event.CreatedDate,
                };

                EventDtoList.Add(EventDTO);
            }

            return new ResponseDTO()
            {
                Message = "Get all events successfully",
                Result = EventDtoList,
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
                var Event = await _unitOfWork.EventRepository.GetById(id);
                if (Event is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Events was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                EventDTO EventDto = new EventDTO()
                {
                    EventId = Event.EventId,
                    WeddingId = Event.WeddingId,
                    BrideName = Event.BrideName,
                    GroomName = Event.GroomName,
                    EventDate = Event.EventDate,
                    EventLocation = Event.EventLocation,
                    EventPhotoUrl = Event.EventPhotoUrl,
                    CreatedDate = Event.CreatedDate,
                };

                return new ResponseDTO()
                {
                    Message = "Get event successfully ",
                    IsSuccess = false,
                    StatusCode = 200,
                    Result = EventDto
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

    public async Task<ResponseDTO> UpdateById(Guid id, UpdateEventDTO updateEventDTO)
    {
        try
        {
            var EventToUpdate = await _unitOfWork.EventRepository.GetById(id);
            if (EventToUpdate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Event was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            EventToUpdate.EventDate = updateEventDTO.EventDate;
            EventToUpdate.EventLocation = updateEventDTO.EventLocation;
            EventToUpdate.EventPhotoUrl = updateEventDTO.EventPhotoUrl;
            EventToUpdate.CreatedDate = updateEventDTO.CreatedDate;

            _unitOfWork.EventRepository.Update(EventToUpdate);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Event updated successfully",
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
            var Event = await _unitOfWork.EventRepository.GetById(id);
            if (Event is null)
            {
                return new ResponseDTO()
                {
                    Message = "Event was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            _unitOfWork.EventRepository.Remove(Event);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Event deleted successfully",
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

    public async Task<ResponseDTO> CreateById(CreateEventDTO createEventDTO)
    {
        try
        {
            var wedding = await _unitOfWork.WeddingRepository.GetById(createEventDTO.WeddingId);

            if (wedding == null)
            {
                return new ResponseDTO
                {
                    Message = "Wedding not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var Event = new Event
            {
                EventId = Guid.NewGuid(),
                WeddingId = createEventDTO.WeddingId,
                EventDate = createEventDTO.EventDate,
                EventLocation = createEventDTO.EventLocation,
                EventPhotoUrl = createEventDTO.EventPhotoUrl,
                CreatedDate = createEventDTO.CreatedDate,
                BrideName = wedding.BrideName,  
                GroomName = wedding.GroomName,
            };

            await _unitOfWork.EventRepository.AddAsync(Event);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Event created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = Event
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
    
    public async Task<ResponseDTO> UploadEventBackground(Guid EventId, UploadEventBackgroundImg uploadEventBackgroundImg)
    {
    try
    {
        if (uploadEventBackgroundImg.File == null)
        {
            return new ResponseDTO()
            {
                IsSuccess = false,
                StatusCode = 400,
                Message = "No file uploaded."
            };
        }

        var Event = await _unitOfWork.EventRepository.GetAsync(x => x.EventId == EventId);
        if (Event == null)
        {
            return new ResponseDTO()
            {
                IsSuccess = false,
                StatusCode = 404,
                Message = "Event not found."
            };
        }
        
        var responseList = new List<string>();
        foreach (var image in uploadEventBackgroundImg.File)
        {
            var filePath = $"{StaticFirebaseFolders.Event}/{Event.EventId}/Background";
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

        Event.EventPhotoUrl = responseList.ToArray();;

        _unitOfWork.EventRepository.Update(Event);
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

public async Task<ResponseDTO> GetEventBackground(Guid EventId)
{
    try
    {
        var Event = await _unitOfWork.EventRepository.GetAsync(x => x.EventId == EventId);

        if (Event != null && Event.EventPhotoUrl.IsNullOrEmpty())
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
            Result = Event.EventPhotos,
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