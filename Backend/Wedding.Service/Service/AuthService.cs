using System.Collections.Concurrent;
using AutoMapper;
using Wedding.Model.Domain;
using Wedding.Model.DTO;
using Wedding.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Wedding.DataAccess.IRepository;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin.Auth;
using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;
using System.Web;
using Newtonsoft.Json;

//using Stripe;

namespace Wedding.Service.Service;

public class AuthService : IAuthService
{
    private readonly IUserManagerRepository _userManagerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IFirebaseService _firebaseService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly ConcurrentDictionary<string, (int Count, DateTime LastRequest)> ResetPasswordAttempts = new();

    public AuthService
    (
        IUserManagerRepository userManagerRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        IEmailService emailService,
        IFirebaseService firebaseService,
        IHttpContextAccessor httpContextAccessor,
        ITokenService tokenService,
        IUnitOfWork unitOfWork
    )
    {
        _userManagerRepository = userManagerRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _emailService = emailService;
        _firebaseService = firebaseService;
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDTO> SignUpCustomer(RegisterCustomerDTO registerCustomerDTO)
    {
        try
        {
            var isEmailExit = await _userManagerRepository.FindByEmailAsync(registerCustomerDTO.Email);
            if (isEmailExit is not null)
            {
                return new ResponseDTO()
                {
                    Message = "Email is using by another user",
                    Result = registerCustomerDTO,
                    IsSuccess = false,
                    StatusCode = 400
                };
            }

            var isPhonenumerExit =
                await _userManagerRepository.CheckIfPhoneNumberExistsAsync(registerCustomerDTO.PhoneNumber);
            if (isPhonenumerExit)
            {
                return new ResponseDTO()
                {
                    Message = "Phone number is using by another user",
                    Result = registerCustomerDTO,
                    IsSuccess = false,
                    StatusCode = 400
                };
            }

            // Create new instance of ApplicationUser
            ApplicationUser newUser = new ApplicationUser()
            {
                Address = registerCustomerDTO.Address,
                Email = registerCustomerDTO.Email,
                BirthDate = registerCustomerDTO.BirthDate,
                UserName = registerCustomerDTO.Email,
                FullName = registerCustomerDTO.FullName,
                Gender = registerCustomerDTO.Gender,
                Country = registerCustomerDTO.Country,
                PhoneNumber = registerCustomerDTO.PhoneNumber,
                UpdateTime = DateTime.Now,
                AvatarUrl = "",
                TaxNumber = "",
                LockoutEnabled = false
            };

            // Create new user to database
            var createUserResult = await _userManagerRepository.CreateAsync(newUser, registerCustomerDTO.Password);

            // Check if error occur
            if (!createUserResult.Succeeded)
            {
                // Return result internal service error
                return new ResponseDTO()
                {
                    Message = createUserResult.Errors.ToString(),
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = registerCustomerDTO
                };
            }

            var user = await _userManagerRepository.FindByPhoneAsync(registerCustomerDTO.PhoneNumber);

            Customer customer = new Customer()
            {
                UserId = user.Id
            };

            var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Customer);

            // Check if role !exist to create new role 
            if (isRoleExist is false)
            {
                await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Customer));
            }

            // Add role for the user
            var isRoleAdd = await _userManagerRepository.AddToRoleAsync(user, StaticUserRoles.Customer);

            if (!isRoleAdd.Succeeded)
            {
                return new ResponseDTO()
                {
                    Message = "Error adding role",
                    IsSuccess = false,
                    StatusCode = 500,
                    Result = registerCustomerDTO
                };
            }

            // Create new Customer relate with ApplicationUser
            var isCustomerAdd = await _unitOfWork.CustomerRepository.AddAsync(customer);
            if (isCustomerAdd == null)
            {
                return new ResponseDTO()
                {
                    Message = "Failed to add customer",
                    IsSuccess = false,
                    StatusCode = 500,
                    Result = registerCustomerDTO
                };
            }

            // Save change to database
            var isSuccess = await _unitOfWork.SaveAsync();
            if (isSuccess <= 0)
            {
                return new ResponseDTO()
                {
                    Message = "Failed to save changes to the database",
                    IsSuccess = false,
                    StatusCode = 500,
                    Result = registerCustomerDTO
                };
            }

            // Return result success
            return new ResponseDTO()
            {
                Message = "Create new user successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = registerCustomerDTO
            };
        }
        catch (Exception e)
        {
            // Return result exception
            return new ResponseDTO()
            {
                Message = e.Message,
                Result = registerCustomerDTO,
                IsSuccess = false,
                StatusCode = 500
            };
        }
    }

    /// <summary>
    /// This method for users to upload their avatar
    /// </summary>
    /// <param name="file">An user avatar image</param>
    /// <param name="user">An user who sent request</param>
    /// <returns></returns>
    public async Task<ResponseDTO> UploadUserAvatar(IFormFile file, ClaimsPrincipal User)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                throw new Exception("Not authentication!");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new Exception("User does not exist");
            }

            var responseDto = await _firebaseService.UploadImage(file, StaticFirebaseFolders.UserAvatars);

            if (!responseDto.IsSuccess)
            {
                throw new Exception("Image upload fail!");
            }

            user.AvatarUrl = responseDto.Result?.ToString();

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new Exception("Update user avatar fail!");
            }

            return new ResponseDTO()
            {
                Message = "Upload user avatar successfully!",
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
                Result = null,
                IsSuccess = false,
                StatusCode = 500
            };
        }
    }

    /// <summary>
    /// This method for user to get their avatar
    /// </summary>
    /// <param name="User">An user who sent request</param>
    /// <returns></returns>
    public async Task<MemoryStream> GetUserAvatar(ClaimsPrincipal User)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByIdAsync(userId);

            var stream = await _firebaseService.GetImage(user.AvatarUrl);

            return stream;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    //Sign In bằng email và password trả về role tương ứng
    public async Task<ResponseDTO> SignIn(SignDTO signDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(signDto.Email);
            if (user == null)
            {
                return new ResponseDTO()
                {
                    Message = "User does not exist!",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, signDto.Password);

            if (!isPasswordCorrect)
            {
                return new ResponseDTO()
                {
                    Message = "Incorrect email or password",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 400
                };
            }

            if (!user.EmailConfirmed)
            {
                return new ResponseDTO()
                {
                    Message = "You need to confirm email!",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 401
                };
            }

            if (user.LockoutEnd is not null)
            {
                return new ResponseDTO()
                {
                    Message = "User has been locked",
                    IsSuccess = false,
                    StatusCode = 403,
                    Result = null
                };
            }

            var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user);
            var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user);
            await _tokenService.StoreRefreshToken(user.Id, refreshToken);

            user.LastLoginTime = DateTime.UtcNow;
            user.SendClearEmail = false;
            await _userManager.UpdateAsync(user);

            return new ResponseDTO()
            {
                Result = new SignByGoogleResponseDTO()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                },
                Message = "Sign in successfully",
                IsSuccess = true,
                StatusCode = 200
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

    /// <summary>
    /// This method for refresh token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<ResponseDTO> Refresh(string token)
    {
        try
        {
            ClaimsPrincipal user = await _tokenService.GetPrincipalFromToken(token);

            var userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return new ResponseDTO()
                {
                    Message = "Token is not valid",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };
            }

            var applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser is null)
            {
                return new ResponseDTO()
                {
                    Message = "User does not exist",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }


            var tokenOnRedis = await _tokenService.RetrieveRefreshToken(applicationUser.Id);
            if (tokenOnRedis != token)
            {
                return new ResponseDTO()
                {
                    Message = "Token is not valid",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };
            }

            var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(applicationUser);
            var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(applicationUser);

            await _tokenService.DeleteRefreshToken(applicationUser.Id);
            await _tokenService.StoreRefreshToken(applicationUser.Id, refreshToken);

            return new ResponseDTO()
            {
                Result = new JwtTokenDTO()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                },
                IsSuccess = true,
                StatusCode = 200,
                Message = "Refresh Token Successfully!"
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

    //Forgot password
    private string ip;
    private string city;
    private string region;
    private string country;
    private const int MaxAttemptsPerDay = 3;

    public async Task<ResponseDTO> ForgotPassword(ForgotPasswordDTO forgotPasswordDto)
    {
        try
        {
            // Tìm người dùng theo Email/Số điện thoại
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.EmailOrPhone);
            if (user == null)
            {
                user = await _userManager.Users.FirstOrDefaultAsync(
                    u => u.PhoneNumber == forgotPasswordDto.EmailOrPhone);
            }

            if (user == null || !user.EmailConfirmed)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "No user found or account not activated.",
                    StatusCode = 400
                };
            }

            // Kiểm tra giới hạn gửi yêu cầu đặt lại mật khẩu
            var email = user.Email;
            var now = DateTime.Now;

            if (ResetPasswordAttempts.TryGetValue(email, out var attempts))
            {
                // Kiểm tra xem đã quá 1 ngày kể từ lần thử cuối cùng chưa
                if (now - attempts.LastRequest >= TimeSpan.FromSeconds(1))
                {
                    // Reset số lần thử về 0 và cập nhật thời gian thử cuối cùng
                    ResetPasswordAttempts[email] = (1, now);
                }
                else if (attempts.Count >= MaxAttemptsPerDay)
                {
                    // Quá số lần reset cho phép trong vòng 1 ngày, gửi thông báo 
                    await _emailService.SendEmailAsync(user.Email,
                        "Password Reset Request Limit Exceeded",
                        $"You have exceeded the daily limit for password reset requests. Please try again after 24 hours."
                    );

                    // Vẫn trong thời gian chặn, trả về lỗi
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        Message =
                            "You have exceeded the daily limit for password reset requests. Please try again after 24 hours.",
                        StatusCode = 429
                    };
                }
                else
                {
                    // Chưa vượt quá số lần thử và thời gian chờ, tăng số lần thử và cập nhật thời gian
                    ResetPasswordAttempts[email] = (attempts.Count + 1, now);
                }
            }
            else
            {
                // Email chưa có trong danh sách, thêm mới với số lần thử là 1 và thời gian hiện tại
                ResetPasswordAttempts.AddOrUpdate(email, (1, now), (key, old) => (old.Count + 1, now));
            }

            // Tạo mã token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Gửi email chứa đường link đặt lại mật khẩu. //reset-password

            var resetLink = $"https://nostran.w3spaces.com?token={token}&email={user.Email}";

            // Lấy ngày hiện tại
            var currentDate = DateTime.Now.ToString("MMMM d, yyyy");

            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"];

            // Lấy tên hệ điều hành
            var operatingSystem = GetUserAgentOperatingSystem(userAgent);

            // Lấy tên trình duyệt
            var browser = GetUserAgentBrowser(userAgent);

            // Lấy location
            var url = "https://ipinfo.io/14.169.10.115/json?token=823e5c403c980f";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(jsonContent);

                    this.ip = data["ip"].ToString();
                    this.city = data["city"].ToString();
                    this.region = data["region"].ToString();
                    this.country = data["country"].ToString();
                }
                else
                {
                    return new ResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Error: Unable to retrieve data.",
                        StatusCode = 400
                    };
                }
            }

            // Gửi email chứa đường link đặt lại mật khẩu
            await _emailService.SendEmailResetAsync(user.Email, "Reset password for your Cursus account", user,
                currentDate, resetLink, operatingSystem, browser, ip, region, city, country);

            // Helper functions (you might need to refine these based on your User-Agent parsing logic)
            string GetUserAgentOperatingSystem(string userAgent)
            {
                // ... Logic to extract the operating system from the user-agent string
                // Example:
                if (userAgent.Contains("Windows")) return "Windows";
                else if (userAgent.Contains("Mac")) return "macOS";
                else if (userAgent.Contains("Linux")) return "Linux";
                else return "Unknown";
            }

            string GetUserAgentBrowser(string userAgent)
            {
                // ... Logic to extract the browser from the user-agent string
                // Example:
                if (userAgent.Contains("Chrome")) return "Chrome";
                else if (userAgent.Contains("Firefox")) return "Firefox";
                else if (userAgent.Contains("Safari")) return "Safari";
                else return "Unknown";
            }

            return new ResponseDTO
            {
                IsSuccess = true,
                Message = "The password reset link has been sent to your email.",
                StatusCode = 200
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Message = e.Message,
                StatusCode = 500
            };
        }
    }

    // Reset password
    public async Task<ResponseDTO> ResetPassword(string email, string token, string password)
    {
        try
        {
            // Tìm người dùng theo email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    StatusCode = 400
                };
            }

            // Kiểm tra xem mật khẩu mới có trùng với mật khẩu cũ hay không
            if (await _userManager.CheckPasswordAsync(user, password))
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "New password cannot be the same as the old password.",
                    StatusCode = 400
                };
            }

            // Xác thực token và reset mật khẩu
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (result.Succeeded)
            {
                return new ResponseDTO
                {
                    IsSuccess = true,
                    Message = "Reset password successfully.",
                    StatusCode = 200
                };
            }
            else
            {
                // Xử lý lỗi nếu token không hợp lệ hoặc có lỗi khác
                StringBuilder errors = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errors.AppendLine(error.Description);
                }

                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = errors.ToString(),
                    StatusCode = 400
                };
            }
        }
        catch (Exception e)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Message = e.Message,
                StatusCode = 500
            };
        }
    }

    // Thay đổi mật khẩu người dùng
    public async Task<ResponseDTO> ChangePassword(string userId, string oldPassword, string newPassword,
        string confirmNewPassword)
    {
        try
        {
            // Lấy id của người dùng
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDTO { IsSuccess = false, Message = "User not found." };
            }

            // Thực hiện xác thực mật khẩu và thay đổi mật khẩu

            // Kiểm tra sự trùng khớp của mật khẩu mới và xác nhận mật khẩu mới 
            if (newPassword != confirmNewPassword)
            {
                return new ResponseDTO
                { IsSuccess = false, Message = "New password and confirm new password not match." };
            }

            // Không cho phép thay đổi mật khẩu cũ
            if (newPassword == oldPassword)
            {
                return new ResponseDTO
                { IsSuccess = false, Message = "New password cannot be the same as the old password." };
            }

            // Thực hiện thay đổi mật khẩu
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
            {
                return new ResponseDTO { IsSuccess = true, Message = "Password changed successfully." };
            }
            else
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "Password change failed. Please ensure the old password is correct."
                };
            }
        }
        catch (Exception e)
        {
            return new ResponseDTO { IsSuccess = false, Message = e.Message };
        }
    }

    /// <summary>
    /// This method for send a token to confirm email
    /// </summary>
    /// <param name="email">Email of user that need to confirm email</param>
    /// <returns></returns>
    public async Task<ResponseDTO> SendVerifyEmail(string email, string confirmationLink)
    {
        try
        {
            await _emailService.SendVerifyEmail(email, confirmationLink);
            return new()
            {
                Message = "Send verify email successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = null
            };
        }
        catch (Exception e)
        {
            return new()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = null
            };
        }
    }

    /// <summary>
    /// This method to verify email has been sent
    /// </summary>
    /// <param name="userId">User that to define who need to confirm email</param>
    /// <param name="token">Token generated by system was sent through email</param>
    /// <returns></returns>
    public async Task<ResponseDTO> VerifyEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user.EmailConfirmed)
        {
            return new ResponseDTO()
            {
                Message = "Your email has been confirmed!",
                IsSuccess = true,
                StatusCode = 200,
                Result = null
            };
        }
        
        string decodedToken = HttpUtility.UrlDecode(token);

        var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (!confirmResult.Succeeded)
        {
            return new()
            {
                Message = confirmResult.Errors.ToString(),
                StatusCode = 400,
                IsSuccess = false,
                Result = null
            };
        }

        return new()
        {
            Message = "Confirm Email Successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = null
        };
    }

    /// <summary>
    /// This method for check email exist or not
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<ResponseDTO> CheckEmailExist(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            return new()
            {
                Result = user is not null,
                Message = user is null ? "Email does not exist" : "Email is existed",
                IsSuccess = true,
                StatusCode = 200
            };
        }
        catch (Exception e)
        {
            return new()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = null
            };
        }
    }

    /// <summary>
    /// This method for check phoneNumber exist or not
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public async Task<ResponseDTO> CheckPhoneNumberExist(string phoneNumber)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            return new()
            {
                Result = user is not null,
                Message = user is null ? "Phone number does not exist!" : "Phone number was existed",
                IsSuccess = true,
                StatusCode = 200
            };
        }
        catch (Exception e)
        {
            return new()
            {
                Message = e.Message,
                Result = null,
                IsSuccess = false,
                StatusCode = 500
            };
        }
    }

    /// <summary>
    /// This method for update customer profile
    /// </summary>
    /// <param name="User"></param>
    /// <param name="customerProfileDto"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ResponseDTO> CompleteCustomerProfile(
        ClaimsPrincipal User,
        CompleteCustomerProfileDTO customerProfileDto)
    {
        try
        {
            // Find user in database
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            // user is null mean user does not exist to update
            if (user is null)
            {
                return new ResponseDTO()
                {
                    Message = "User does not exist!",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = customerProfileDto,
                };
            }

            // Check if phone number is user by another user but not the user to update
            var isPhonenumerExit =
                await _userManager.Users.AnyAsync(
                    u => u.PhoneNumber == customerProfileDto.PhoneNumber && u.Id != user.Id);
            if (isPhonenumerExit)
            {
                return new ResponseDTO()
                {
                    Message = "Phone number is using by another user",
                    Result = customerProfileDto,
                    IsSuccess = false,
                    StatusCode = 500
                };
            }

            user.BirthDate = customerProfileDto.BirthDate;
            user.PhoneNumber = customerProfileDto.PhoneNumber;
            user.Address = customerProfileDto.Address;
            user.Country = customerProfileDto.Country;
            user.Gender = customerProfileDto.Gender;

            var customer = await _unitOfWork.CustomerRepository.GetAsync(x => x.UserId == userId);
            if (customer is null)
            {
                customer = new Customer()
                {
                    UserId = user.Id
                };
                await _unitOfWork.CustomerRepository.AddAsync(customer);
            }
            else
            {
                
            }

            var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Customer);

            // Check if role !exist to create new role 
            if (isRoleExist is false)
            {
                await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Customer));
            }

            // Add role for the user
            await _userManager.AddToRoleAsync(user, StaticUserRoles.Customer);

            await _unitOfWork.SaveAsync();

            return new ResponseDTO()
            {
                Message = "Update customer profile successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = customerProfileDto
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

    public async Task<ResponseDTO> GetUserInfo(ClaimsPrincipal User)
    {
        // Find user in database
        try
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            // user is null mean user does not exist to update
            if (user is null)
            {
                return new ResponseDTO()
                {
                    Message = "User does not exist!",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null,
                };
            }

            var userInfo = _mapper.Map<UserInfoDTO>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userInfo.Roles = roles;
            userInfo.isAccepted = true;

            if (roles.Contains(StaticUserRoles.Customer))
            {
                var customer = await _unitOfWork.CustomerRepository.GetByUserId(userId);
                userInfo.CustomerId = customer?.CustomerId;
            }

            return new ResponseDTO()
            {
                Message = "Get user info successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = userInfo,
            };
        }
        catch (Exception e)
        {
            return new ResponseDTO()
            {
                Message = "Something went wrong",
                IsSuccess = false,
                StatusCode = 500,
                Result = null,
            };
        }
    }

    public async Task<MemoryStream> DisplayUserAvatar(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);

            var stream = await _firebaseService.GetImage(user.AvatarUrl);

            return stream;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    /// <summary>
/// Sign in a User by Google.
/// </summary>
/// <param name="signInByGoogleDto"></param>
/// <returns></returns>
public async Task<ResponseDTO> SignInByGoogle(SignInByGoogleDTO signInByGoogleDTO)
{
    // Gọi API của Google để lấy thông tin từ Access Token
    var httpClient = new HttpClient();
    var response =
        await httpClient.GetStringAsync(
            $"https://www.googleapis.com/oauth2/v3/tokeninfo?access_token={signInByGoogleDTO.Token}");

    // Parse response từ Google
    var googleUser = JsonConvert.DeserializeObject<GoogleUserInfo>(response);
    if (googleUser == null || googleUser.email == null)
    {
        return new ResponseDTO()
        {
            Message = "Invalid Google Access Token",
            IsSuccess = false,
            StatusCode = 401
        };
    }

    string email = googleUser.email;

    // Tìm kiếm người dùng trong database
    var user = await _userManager.FindByEmailAsync(email);
    UserLoginInfo? userLoginInfo = null;
    if (user is not null)
    {
        userLoginInfo = (await _userManager.GetLoginsAsync(user))
            .FirstOrDefault(x => x.LoginProvider == StaticLoginProvider.Google);
    }

    if (user?.LockoutEnd is not null)
    {
        return new ResponseDTO()
        {
            Message = "User has been locked",
            IsSuccess = false,
            StatusCode = 403,
            Result = null
        };
    }

    if (user is not null && userLoginInfo is null)
    {
        return new ResponseDTO()
        {
            Result = new SignByGoogleResponseDTO()
            {
                RefreshToken = "",
                AccessToken = "",
            },
            Message = "The email is using by another user",
            IsSuccess = false,
            StatusCode = 400
        };
    }

    // Nếu user chưa tồn tại, tạo user mới và thêm role "Member"
    if (user is null)
    {
        user = new ApplicationUser
        {
            Email = email,
            FullName = "",
            UserName = email,
            AvatarUrl = "",
            EmailConfirmed = true,
            UpdateTime = null
        };

        // Tạo user mới trong database
        var createUserResult = await _userManager.CreateAsync(user);
        if (!createUserResult.Succeeded)
        {
            return new ResponseDTO()
            {
                Message = "Error creating user",
                IsSuccess = false,
                StatusCode = 400
            };
        }

        // Thêm thông tin đăng nhập Google vào tài khoản
        await _userManager.AddLoginAsync(user, new UserLoginInfo(StaticLoginProvider.Google, googleUser.sub, "GOOGLE"));

        // Kiểm tra và tạo role "Member" nếu chưa có
        var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Customer);
        if (!isRoleExist)
        {
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Customer));
        }

        // Thêm role "Member" cho người dùng mới
        var isRoleAdded = await _userManager.AddToRoleAsync(user, StaticUserRoles.Customer);
        if (!isRoleAdded.Succeeded)
        {
            return new ResponseDTO()
            {
                Message = "Error adding role",
                IsSuccess = false,
                StatusCode = 500
            };
        }
    }

    // Cập nhật thông tin người dùng
    await _userManager.UpdateAsync(user);

    // Kiểm tra thông tin bắt buộc đã được cập nhật chưa
    bool isProfileComplete =
        !string.IsNullOrEmpty(user.FullName) &&
        !string.IsNullOrEmpty(user.AvatarUrl);

    // Tạo Access Token và Refresh Token cho user
    var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user!);
    var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user!);
    await _tokenService.StoreRefreshToken(user!.Id, refreshToken);

    // Nếu hồ sơ chưa hoàn chỉnh, trả về cảnh báo
    if (!isProfileComplete)
    {
        return new ResponseDTO()
        {
            Result = new SignByGoogleResponseDTO()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IsProfileComplete = false // Thông báo nếu cần cập nhật thông tin
            },
            Message = "Your profile is incomplete. Please update your profile information.",
            IsSuccess = true, // Vẫn trả về thành công để cấp quyền truy cập
            StatusCode = 200
        };
    }

    // Nếu thông tin đầy đủ
    return new ResponseDTO()
    {
        Result = new SignByGoogleResponseDTO()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            IsProfileComplete = true
        },
        Message = "Sign in successfully",
        IsSuccess = true,
        StatusCode = 200
    };
}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="updateCustomerDto"></param>
    /// <returns></returns>
    public async Task<ResponseDTO> UpdateCustomer(UpdateCustomerProfileDTO updateCustomerDTO, ClaimsPrincipal User)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                return new ResponseDTO
                {
                    Message = "User not found",
                    Result = null,
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var customerToUpdate = await _unitOfWork.CustomerRepository.GetByUserId(userId);

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

            // Update related ApplicationUser fields
            customerToUpdate.ApplicationUser.Address = updateCustomerDTO.Address;
            customerToUpdate.ApplicationUser.BirthDate = updateCustomerDTO.BirthDate;
            customerToUpdate.ApplicationUser.Gender = updateCustomerDTO.Gender;
            customerToUpdate.ApplicationUser.FullName = updateCustomerDTO.FullName;
            customerToUpdate.ApplicationUser.Country = updateCustomerDTO.Country;

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

    public async Task<ResponseDTO> LockUser(LockUserDTO lockUserDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(lockUserDto.UserId);


            if (user is null)
            {
                return new ResponseDTO()
                {
                    Message = "User was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var userRole = await _userManager.GetRolesAsync(user);

            if (userRole.Contains(StaticUserRoles.Admin))
            {
                return new ResponseDTO()
                {
                    Message = "Lock user was failed",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };
            }

            user.LockoutEnd = DateTimeOffset.MaxValue;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new ResponseDTO()
                {
                    Message = "Lock user was failed",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };
            }

            return new ResponseDTO()
            {
                Message = "Lock user successfully",
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

    public async Task<ResponseDTO> UnlockUser(LockUserDTO lockUserDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(lockUserDto.UserId);

            if (user is null)
            {
                return new ResponseDTO()
                {
                    Message = "User was not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            user.LockoutEnd = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new ResponseDTO()
                {
                    Message = "Unlock user was failed",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };
            }

            return new ResponseDTO()
            {
                Message = "Unlock user successfully",
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

    public async Task SendClearEmail(int fromMonth)
    {
        try
        {
            var fromDate = DateTime.UtcNow.AddMonths(-fromMonth);
            var users = _userManager.Users
                .Where(user => user.LastLoginTime <= fromDate || user.LastLoginTime == null)
                .ToList();

            var admins = await _userManager.GetUsersInRoleAsync(StaticUserRoles.Admin);

            foreach (var admin in admins)
            {
                users.Remove(admin);
            }

            if (users.IsNullOrEmpty())
            {
                return;
            }

            foreach (var user in users)
            {
                if (user.Email is null) continue;
                var result = await _emailService.SendEmailRemindDeleteAccount(user.Email);
                if (result)
                {
                    user.SendClearEmail = true;
                    await _userManager.UpdateAsync(user);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task ClearUser()
    {
        try
        {
            var users = _userManager.Users.Where(user => user.SendClearEmail == true).ToList();

            var customers = new List<Customer>();

            foreach (var user in users)
            {
                await _emailService.SendEmailDeleteAccount(user.Email);
                var role = await _userManager.GetRolesAsync(user);

                if (role.Contains(StaticUserRoles.Customer))
                {
                    var customer = await _unitOfWork.CustomerRepository.GetAsync(x => x.UserId == user.Id);
                    customers.Add(customer);
                }
            }

            _unitOfWork.CustomerRepository.RemoveRange(customers);

            foreach (var user in users)
            {
                await DeleteUserAndRelatedDataAsync(user);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task DeleteUserAndRelatedDataAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, roles);
        }

        var claims = await _userManager.GetClaimsAsync(user);
        if (claims.Any())
        {
            foreach (var claim in claims)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }
        }

        var logins = await _userManager.GetLoginsAsync(user);
        if (logins.Any())
        {
            foreach (var login in logins)
            {
                await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
            }
        }

        var tokens = await _userManager.GetAuthenticationTokenAsync(user, "provider", "name");
        if (tokens != null)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "provider", "name");
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Error deleting user: {error.Description}");
            }
        }
    }
}
