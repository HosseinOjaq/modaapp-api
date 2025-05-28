namespace ModaApp.Api.Attributes;

/// <summary>
/// it is used whe we need a method that requires authentication,
/// but we do not want it to be included in our dynamic permission list.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class NoPermissionAttribute : Attribute { }