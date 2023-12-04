using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PinewoodTest.CustomerService.Api.Models;
using Dtos;

public class Customer
{
    public Guid? Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    public Customer() { }

    [SetsRequiredMembers]
    internal Customer(
        CustomerDto dto
    )
    {
        Id = dto.Id;
        Name = dto.Name;
        IsActive = dto.IsActive;
    }

    internal CustomerDto ToDto()
        => new(
            Id,
            Name,
            IsActive
        );
}