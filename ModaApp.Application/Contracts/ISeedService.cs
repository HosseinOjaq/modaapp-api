using ModaApp.Application.Models;

namespace ModaApp.Application.Common.Contracts;

public interface ISeedService
{
    Task SeedDataAsync(List<DynamicPermission> permissions);
}