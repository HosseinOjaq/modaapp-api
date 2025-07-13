﻿namespace ModaApp.Domain.Configuration.SMS;

public interface IPhoneNumberTokenProviderConfiguration
{
    int MinNumber { get; set; }
    int MaxNumber { get; set; }
    int Duration { get; set; }
    int DigitCount { get; set; }
}