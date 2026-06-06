using TaxRacm.Controls.Domain.ValueObjects;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Controls.Domain.Entities;

public class Evidence : Entity<Guid>
{
    public ControlId ControlId { get; private set; } = null!;
    public string FileName { get; private set; } = string.Empty;
    public string FileUrl { get; private set; } = string.Empty;
    public UserId UploadedById { get; private set; } = null!;
    public string Notes { get; private set; } = string.Empty;
    public DateTime UploadedAt { get; private set; }

    private Evidence() { }

    public static Evidence Create(ControlId controlId, string fileName, string fileUrl, UserId uploadedBy, string notes) => new()
    {
        Id = Guid.NewGuid(),
        ControlId = controlId,
        FileName = fileName,
        FileUrl = fileUrl,
        UploadedById = uploadedBy,
        Notes = notes,
        UploadedAt = DateTime.UtcNow
    };
}
