using System.ComponentModel.DataAnnotations;

namespace ModaApp.Common.Enums;

public enum OperationStatusCode
{
    [Display(Name = "عملیات با موفقیت انجام شد")]
    OK = 200,

    [Display(Name = "خطایی در پردازش رخ داد")]
    ServerError = 500,

    [Display(Name = "درخواست نامعتبر")]
    BadRequest = 400,

    [Display(Name = "یافت نشد")]
    NotFound = 404,

    [Display(Name = "خطای احراز هویت")]
    UnAuthorized = 401
}