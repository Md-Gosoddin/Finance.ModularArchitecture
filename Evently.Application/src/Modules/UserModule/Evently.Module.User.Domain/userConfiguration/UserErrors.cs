using BuildingBlock.Domain;

namespace Evently.Module.User.Domain.userConfiguration;
public static class UserErrors
{
    public static Error NotFound(Guid userId) =>
       Error.NotFound("Users.NotFound", $"The user with the identifier {userId} not found");

    public static Error NotFound(string identityId) =>
        Error.NotFound("Users.NotFound", $"The user with the IDP identifier {identityId} not found");

    public static Error DataNotAvailable =>
        Error.NotFound("No Record Found", "Record Not available..");
}
