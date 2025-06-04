using System.Security.Claims;
using Wedding.Model.DTO;
using Microsoft.AspNetCore.Http;

namespace Wedding.Service.IService;

public interface IAuthService
{
    Task<ResponseDTO> SignUpCustomer(RegisterCustomerDTO registerCustomerDTO);
    Task<ResponseDTO> UploadUserAvatar(IFormFile file, ClaimsPrincipal user);
    Task<MemoryStream> GetUserAvatar(ClaimsPrincipal user);
    Task<ResponseDTO> SignIn(SignDTO signDto);
    Task<ResponseDTO> ForgotPassword(ForgotPasswordDTO forgotPasswordDto);
    Task<ResponseDTO> Refresh(string token);
    Task<ResponseDTO> ResetPassword(string resetPasswordDto, string token, string password);
    Task<ResponseDTO> ChangePassword(string userId, string oldPassword, string newPassword, string confirmNewPassword);
    Task<ResponseDTO> SendVerifyEmail(string email, string confirmationLink);
    Task<ResponseDTO> VerifyEmail(string userId, string token);
    Task<ResponseDTO> CheckEmailExist(string email);
    Task<ResponseDTO> CheckPhoneNumberExist(string phoneNumber);
    Task<ResponseDTO> SignInByGoogle(SignInByGoogleDTO signInByGoogleDto);
    Task<ResponseDTO> CompleteCustomerProfile(ClaimsPrincipal User, CompleteCustomerProfileDTO customerProfileDto);
    Task<ResponseDTO> GetUserInfo(ClaimsPrincipal User);
    Task<MemoryStream> DisplayUserAvatar(string userId);
    Task<ResponseDTO> UpdateCustomer(UpdateCustomerProfileDTO updateCustomerDto, ClaimsPrincipal User);
    Task<ResponseDTO> LockUser(LockUserDTO lockUserDto);
    Task<ResponseDTO> UnlockUser(LockUserDTO lockUserDto);
    Task SendClearEmail(int fromMonth);
    Task ClearUser();
}
