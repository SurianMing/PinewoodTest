namespace PinewoodTest.CustomerService.Data.Mappers;
using Context.EntityModels;
using Dtos;

internal static class CustomerMapper
{
    internal static CustomerDto ToDto(this Customer entity)
        => new(
            entity.Id,
            entity.Name,
            entity.IsActive
        );

    internal static Customer ToEntity(this CustomerDto dto)
        => new()
        {
            Id = dto.Id!.Value,
            Name = dto.Name,
            IsActive = dto.IsActive
        };
}