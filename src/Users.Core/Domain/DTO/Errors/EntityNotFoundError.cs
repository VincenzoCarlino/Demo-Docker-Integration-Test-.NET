namespace Users.Core.Domain.DTO.Errors;
using System;

public class EntityNotFoundError
{
    public string EntityName { get; }
    public string PropertyName { get; }
    public object PropertyValue { get; }

    public EntityNotFoundError(string entityName, string propertyName, object propertyValue)
    {
        EntityName = entityName;
        PropertyName = propertyName;
        PropertyValue = propertyValue;
    }

    public static EntityNotFoundError UserNotFoundById(Guid id)
        => new(
            "user",
            "id",
            id
        );
}
