﻿namespace ModaApp.Application.Common.Contracts;

public interface ISmsService
{
    bool Send(string phoneNumber, string message);
    Task<string> GenerateTokenAsync(string phoneNumber);
    Task<bool> VerifyTokenAsync(string token, string phoneNumber);
}