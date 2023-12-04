namespace PinewoodTest.CustomerService.Dtos;

public class CustomerDto(
    string name,
    bool isActive = true
)
{
    internal Guid? Id { get; private set; }
    internal string Name { get; } = name;
    internal bool IsActive { get; } = isActive;

    internal CustomerDto(
        Guid? id,
        string name,
        bool isActive
    ) : this(name, isActive)
    {
        Id = id;
    }

    internal void SetId(Guid newId) => Id = newId;
}