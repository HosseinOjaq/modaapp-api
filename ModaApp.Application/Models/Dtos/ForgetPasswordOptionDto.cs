using ModaApp.Domain.Enums;

namespace ModaApp.Application.Models.Dtos;

public record ForgetPasswordOptionDto
(
    string Title,
    ForgetPasswordOptionType OptionType
);