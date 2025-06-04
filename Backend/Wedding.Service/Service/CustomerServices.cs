using AutoMapper;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Wedding.Model.DTO;
using Wedding.Service.Hubs;
using Wedding.Service.IService;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Wedding.Utility.Constants;

namespace Wedding.Service.Service
{
    public class CustomerService : ICustomersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClosedXMLService _closedXmlService;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper,
            IClosedXMLService closedXmlService, IHubContext<NotificationHub> notificationHub,
            IWebHostEnvironment env, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _closedXmlService = closedXmlService;
            _notificationHub = notificationHub;
            _env = env;
            _config = config;
        }

        //Get all Customers đã xong
        public async Task<ResponseDTO> GetAllCustomer
        (
            ClaimsPrincipal User,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool? isAscending,
            int pageNumber,
            int pageSize
        )
        {
            #region MyRegion

            try
            {
                List<Customer> customers = new List<Customer>();

                // Filter Query
                if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
                {
                    switch (filterOn.Trim().ToLower())
                    {
                        case "name":
                        {
                            customers = _unitOfWork.CustomerRepository.GetAllAsync(includeProperties: "ApplicationUser")
                                .GetAwaiter().GetResult().Where(x =>
                                    x.ApplicationUser.FullName.Contains(filterQuery,
                                        StringComparison.CurrentCultureIgnoreCase)).ToList();
                            break;
                        }
                        case "email":
                        {
                            customers = _unitOfWork.CustomerRepository.GetAllAsync(includeProperties: "ApplicationUser")
                                .GetAwaiter().GetResult().Where(x =>
                                    x.ApplicationUser.Email.Contains(filterQuery,
                                        StringComparison.CurrentCultureIgnoreCase)).ToList();
                            break;
                        }
                        default:
                        {
                            customers = _unitOfWork.CustomerRepository.GetAllAsync(includeProperties: "ApplicationUser")
                                .GetAwaiter().GetResult().ToList();
                            break;
                        }
                    }
                }
                else
                {
                    customers = _unitOfWork.CustomerRepository.GetAllAsync(includeProperties: "ApplicationUser")
                        .GetAwaiter().GetResult().ToList();
                }

                // Sort Query
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.Trim().ToLower())
                    {
                        case "name":
                        {
                            customers = isAscending == true
                                ? [.. customers.OrderBy(x => x.ApplicationUser.FullName)]
                                : [.. customers.OrderByDescending(x => x.ApplicationUser.FullName)];
                            break;
                        }
                        case "email":
                        {
                            customers = isAscending == true
                                ? [.. customers.OrderBy(x => x.ApplicationUser.Email)]
                                : [.. customers.OrderByDescending(x => x.ApplicationUser.Email)];
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
                    customers = customers.Skip(skipResult).Take(pageSize).ToList();
                }

                #endregion Query Parameters

                if (customers == null || !customers.Any())
                {
                    return new ResponseDTO()
                    {
                        Message = "There are no Customers",
                        Result = null,
                        IsSuccess = false,
                        StatusCode = 404
                    };
                }

                var customerInfoDtoList = new List<CustomerInfoDTO>();

                foreach (var customer in customers)
                {
                    var customerInfoDto = new CustomerInfoDTO
                    {
                        CustomerId = customer.CustomerId,
                        FullName = customer.ApplicationUser?.FullName,
                        Email = customer.ApplicationUser?.Email,
                        PhoneNumber = customer.ApplicationUser?.PhoneNumber,
                    };

                    customerInfoDtoList.Add(customerInfoDto);
                }

                return new ResponseDTO()
                {
                    Message = "Get all customers successfully",
                    Result = customerInfoDtoList,
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

        //GetById đã xong
        public async Task<ResponseDTO> GetById(Guid id)
        {
            {
                try
                {
                    var customer = await _unitOfWork.CustomerRepository.GetById(id);
                    if (customer is null)
                    {
                        return new ResponseDTO()
                        {
                            Message = "Customer was not found",
                            IsSuccess = false,
                            StatusCode = 404,
                            Result = null
                        };
                    }

                    CustomerFullInfoDTO customerFullInfoDto = new CustomerFullInfoDTO()
                    {
                        CustomerId = customer.CustomerId,
                        UserId = customer.UserId,
                        AvatarUrl = customer.ApplicationUser.AvatarUrl,
                        FullName = customer.ApplicationUser.FullName,
                        Email = customer.ApplicationUser.Email,
                        Address = customer.ApplicationUser.Address,
                        BirthDate = customer.ApplicationUser.BirthDate,
                        Country = customer.ApplicationUser.Country,
                        Gender = customer.ApplicationUser.Gender,
                        PhoneNumber = customer.ApplicationUser.PhoneNumber,
                    };

                    return new ResponseDTO()
                    {
                        Message = "Get customer successfully ",
                        IsSuccess = false,
                        StatusCode = 200,
                        Result = customerFullInfoDto
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

        //UpdateById đã xong
        public async Task<ResponseDTO> UpdateById(UpdateCustomerDTO updateCustomerDTO)
        {
            try
            {
                var customerToUpdate =
                    await _unitOfWork.CustomerRepository.GetById(updateCustomerDTO.CustomerId);

                if (customerToUpdate == null)
                {
                    return new ResponseDTO
                    {
                        Message = "Customer not found",
                        Result = null,
                        IsSuccess = false,
                        StatusCode = 404
                    };
                }

                customerToUpdate.ApplicationUser.Address = updateCustomerDTO?.Address;
                customerToUpdate.ApplicationUser.BirthDate = updateCustomerDTO?.BirthDate;
                customerToUpdate.ApplicationUser.Gender = updateCustomerDTO?.Gender;
                customerToUpdate.ApplicationUser.Country = updateCustomerDTO?.Country;
                customerToUpdate.ApplicationUser.UpdateTime = DateTime.Now;


                _unitOfWork.CustomerRepository.Update(customerToUpdate);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO
                {
                    Message = "Customer updated successfully",
                    Result = null,
                    IsSuccess = true,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                return new ResponseDTO
                {
                    Message = e.Message,
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 500
                };
            }
        }

        //activate Customer đã xong
        public async Task<ResponseDTO> ActivateCustomer(ClaimsPrincipal User, Guid customerId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var customer = await _unitOfWork.CustomerRepository.GetById(customerId);
                if (customer is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Customer was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                customer.ApplicationUser.LockoutEnabled = true;
                customer.ApplicationUser.LockoutEnd = DateTime.UtcNow;
                await _unitOfWork.SaveAsync();

                return new ResponseDTO()
                {
                    Message = "Active customer successfully",
                    Result = null,
                    IsSuccess = true,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                return new ResponseDTO()
                {
                    Message = e.Message,
                    IsSuccess = true,
                    StatusCode = 500,
                    Result = null
                };
            }
        }

        //Deactivate Customer đã xong
        public async Task<ResponseDTO> DeactivateCustomer(ClaimsPrincipal User, Guid customerId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                var customer = await _unitOfWork.CustomerRepository.GetById(customerId);
                if (customer is null)
                {
                    return new ResponseDTO()
                    {
                        Message = "Customer was not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Result = null
                    };
                }

                customer.ApplicationUser.LockoutEnabled = false;
                await _unitOfWork.SaveAsync();

                return new ResponseDTO()
                {
                    Message = "Reject customer successfully",
                    Result = null,
                    IsSuccess = true,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                return new ResponseDTO()
                {
                    Message = e.Message,
                    IsSuccess = true,
                    StatusCode = 500,
                    Result = null
                };
            }
        }

        //ExportCustomers đã xong
        public async Task<ResponseDTO> ExportCustomers(string userId, int month, int year)
        {
            // Lấy dữ liệu từ repository
            var customers = _unitOfWork.CustomerRepository.GetAllAsync(includeProperties: "ApplicationUser")
                .GetAwaiter().GetResult().ToList();

            // Lọc dữ liệu theo tháng và năm
            customers = customers.Where(x =>
                    x.ApplicationUser.CreateTime.HasValue && x.ApplicationUser.CreateTime.Value.Month == month &&
                    x.ApplicationUser.CreateTime.Value.Year == year)
                .ToList();

            // Map dữ liệu sang DTO
            var customerInfoDtos = _mapper.Map<List<CustomerFullInfoDTO>>(customers);

            // Xuất file Excel
            var fileCustomer = await _closedXmlService.ExportCustomerExcel(customerInfoDtos);

            // Gửi tín hiệu cho người dùng sau khi xuất file
            await _notificationHub.Clients.User(userId).SendAsync("DownloadExcelNow", fileCustomer);

            return new ResponseDTO()
            {
                Message = "Waiting...",
                IsSuccess = true,
                StatusCode = 200,
                Result = null
            };
        }

        //DownloadCustomers đã xong
        public async Task<ClosedXMLResponseDTO> DownloadCustomers(string fileName)
        {
            try
            {
                string filePath = Path.Combine(_env.ContentRootPath, _config["FolderPath:CustomerExportFolderPath"],
                    fileName);

                if (!File.Exists(filePath))
                {
                    return new ClosedXMLResponseDTO()
                    {
                        Message = "File not found",
                        IsSuccess = false,
                        StatusCode = 404,
                        Stream = null,
                        ContentType = null,
                        FileName = null
                    };
                }

                // Read the file
                var memoryStream = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memoryStream);
                }

                memoryStream.Position = 0;
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                //Delete file after download
                File.Delete(filePath);

                return new ClosedXMLResponseDTO()
                {
                    Message = "Download file successfully",
                    IsSuccess = true,
                    StatusCode = 200,
                    Stream = memoryStream,
                    ContentType = contentType,
                    FileName = fileName
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}