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

public class WeddingService : IWeddingService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFirebaseService _firebaseService;

    public WeddingService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
        IFirebaseService firebaseService)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _firebaseService = firebaseService;
    }

    public async Task<ResponseDTO> GetAll(ClaimsPrincipal User, string? filterOn, string? filterQuery, string? sortBy,
        bool? isAscending,
        int pageNumber, int pageSize)
    {
        #region MyRegion

        try
        {
            List<Model.Domain.Wedding> weddings = new List<Model.Domain.Wedding>();

            // Filter Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                switch (filterOn.Trim().ToLower())
                {
                    case "name":
                    {
                        weddings = _unitOfWork.WeddingRepository.GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().Where(x =>
                                x.ApplicationUser.FullName.Contains(filterQuery,
                                    StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    }
                    case "weddinglocation":
                    {
                        weddings = _unitOfWork.WeddingRepository.GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().Where(x =>
                                x.WeddingLocation.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase))
                            .ToList();
                        break;
                    }
                    default:
                    {
                        weddings = _unitOfWork.WeddingRepository
                            .GetAllAsync(includeProperties: "ApplicationUser")
                            .GetAwaiter().GetResult().ToList();
                        break;
                    }
                }
            }
            else
            {
                weddings = _unitOfWork.WeddingRepository.GetAllAsync(includeProperties: "ApplicationUser")
                    .GetAwaiter().GetResult().ToList();
            }

            // Sort Query
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "name":
                    {
                        weddings = isAscending == true
                            ? weddings.OrderBy(x => x.ApplicationUser.FullName).ToList()
                            : weddings.OrderByDescending(x => x.ApplicationUser.FullName).ToList();
                        break;
                    }
                    case "weddinglocation":
                    {
                        weddings = isAscending == true
                            ? weddings.OrderBy(x => x.WeddingLocation).ToList()
                            : weddings.OrderByDescending(x => x.WeddingLocation).ToList();
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
                weddings = weddings.Skip(skipResult).Take(pageSize).ToList();
            }

            #endregion Query Parameters

            if (weddings == null || !weddings.Any())
            {
                return new ResponseDTO()
                {
                    Message = "No weddings found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var weddingDtoList = new List<WeddingDTO>();

            foreach (var wedding in weddings)
            {
                var weddingDTO = new WeddingDTO
                {
                    WeddingId = wedding.WeddingId,
                    UserId = wedding.ApplicationUser?.FullName,
                    BrideName = wedding.BrideName,
                    GroomName = wedding.GroomName,
                    WeddingDate = wedding.WeddingDate,
                    WeddingLocation = wedding.WeddingLocation,
                    WeddingPhotoUrl = wedding.WeddingPhotoUrl,
                    CreatedDate = wedding.CreatedDate,
                };

                weddingDtoList.Add(weddingDTO);
            }

            return new ResponseDTO()
            {
                Message = "Get all weddings successfully",
                Result = weddingDtoList,
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
                var wedding = await _unitOfWork.WeddingRepository.GetById(id);
                if (wedding is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Wedding was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                WeddingDTO weddingDto = new WeddingDTO()
                {
                    WeddingId = wedding.WeddingId,
                    UserId = wedding.UserId,
                    BrideName = wedding.BrideName,
                    GroomName = wedding.GroomName,
                    WeddingDate = wedding.WeddingDate,
                    WeddingLocation = wedding.WeddingLocation,
                    WeddingPhotoUrl = wedding.WeddingPhotoUrl,
                    CreatedDate = wedding.CreatedDate
                };

                return new ResponseDTO()
                {
                    Message = "Get wedding successfully ",
                    IsSuccess = false,
                    StatusCode = 200,
                    Result = weddingDto
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

    public async Task<ResponseDTO> UpdateById(Guid id, UpdateWeddingDTO updateWeddingDTO)
    {
        try
        {
            var weddingToUpdate = await _unitOfWork.WeddingRepository.GetById(id);
            if (weddingToUpdate is null)
            {
                return new ResponseDTO()
                {
                    Message = "Wedding was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            weddingToUpdate.BrideName = updateWeddingDTO.BrideName;
            weddingToUpdate.GroomName = updateWeddingDTO.GroomName;
            weddingToUpdate.WeddingDate = updateWeddingDTO.WeddingDate;
            weddingToUpdate.WeddingLocation = updateWeddingDTO.WeddingLocation;
            weddingToUpdate.WeddingPhotoUrl = updateWeddingDTO.WeddingPhotoUrl;
            weddingToUpdate.CreatedDate = updateWeddingDTO.CreatedDate;

            _unitOfWork.WeddingRepository.Update(weddingToUpdate);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Wedding updated successfully",
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
            var wedding = await _unitOfWork.WeddingRepository.GetById(id);
            if (wedding is null)
            {
                return new ResponseDTO()
                {
                    Message = "Wedding was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            _unitOfWork.WeddingRepository.Remove(wedding);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Wedding deleted successfully",
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

    public async Task<ResponseDTO> CreateById(CreateWeddingDTO createWeddingDTO)
    {
        try
        {
            var wedding = new Model.Domain.Wedding()
            {
                WeddingId = Guid.NewGuid(),
                UserId = createWeddingDTO.UserId,
                BrideName = createWeddingDTO.BrideName,
                GroomName = createWeddingDTO.GroomName,
                WeddingDate = createWeddingDTO.WeddingDate,
                WeddingLocation = createWeddingDTO.WeddingLocation,
                WeddingPhotoUrl = createWeddingDTO.WeddingPhotoUrl,
                CreatedDate = createWeddingDTO.CreatedDate
            };

            await _unitOfWork.WeddingRepository.AddAsync(wedding);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Wedding created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = wedding
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

    public async Task<ResponseDTO> UploadWeddingBackground(Guid WeddingId,
        UploadWeddingBackgroundImg uploadCourseVersionBackgroundImg)
    {
        try
        {
            if (uploadCourseVersionBackgroundImg.File == null)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "No file uploaded."
                };
            }

            var wedding = await _unitOfWork.WeddingRepository.GetAsync(x => x.WeddingId == WeddingId);
            if (wedding == null)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Wedding not found."
                };
            }
            
            var responseList = new List<string>();
            foreach (var image in uploadCourseVersionBackgroundImg.File)
            {
                var filePath = $"{StaticFirebaseFolders.EventPhoto}/{wedding.WeddingId}/Background";
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

            wedding.WeddingPhotoUrl = responseList.ToArray();;

            _unitOfWork.WeddingRepository.Update(wedding);
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

    public async Task<ResponseDTO> GetWeddingBackground(Guid WeddingId)
    {
        try
        {
            var wedding = await _unitOfWork.WeddingRepository.GetAsync(x => x.WeddingId == WeddingId);

            if (wedding != null && wedding.WeddingPhotoUrl.IsNullOrEmpty())
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
                Result = wedding.WeddingPhotoUrl,
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