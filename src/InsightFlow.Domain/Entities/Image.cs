using InsightFlow.Domain.Common;
using InsightFlow.Domain.Enums;

namespace InsightFlow.Domain.Entities;

public class Image : DomainEntity
{
    public byte[]? ImageBytes { get; set; }

    public string? ImageFormat { get; set; }

    public long OwnerId { get; init; }

    public required ImageType Type { get; set; }
}