using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace QuickCode.MyecommerceDemo.Common.Models; 

public class LoginRequest
{
    [EmailAddress]
    [Description("Email address of the user")]
    [DefaultValue("demo@quickcode.net")]
    public string Email { get; set; }

    [Description("User's password")]
    [DefaultValue("String1!")]
    public string Password { get; set; }

    [Description("Two-factor authentication code, if applicable")]
    [DefaultValue(null)]
    public string? TwoFactorCode { get; set; }

    [Description("Two-factor recovery code, if applicable")]
    [DefaultValue(null)]
    public string? TwoFactorRecoveryCode { get; set; }
}