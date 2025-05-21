

using System.ComponentModel.DataAnnotations;

namespace ModaApp.Domain.Enums;

public enum PermissionType : int
{
    [Display(Name = "مدیر")]
    Admin = 1,
}