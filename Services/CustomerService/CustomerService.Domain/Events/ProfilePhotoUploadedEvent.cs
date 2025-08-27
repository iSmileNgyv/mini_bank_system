namespace CustomerService.Domain.Events;

public class ProfilePhotoUploadedEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public int CustomerId { get; }
    public string FileName { get; }
    public long FileSize { get; }
    public string ContentType { get; }
    public int Width { get; }
    public int Height { get; }
    public string InternalUrl { get; }

    public ProfilePhotoUploadedEvent(
        int customerId, 
        string fileName, 
        long fileSize, 
        string contentType,
        int width,
        int height,
        string internalUrl)
    {
        CustomerId = customerId;
        FileName = fileName;
        FileSize = fileSize;
        ContentType = contentType;
        Width = width;
        Height = height;
        InternalUrl = internalUrl;
    }
}