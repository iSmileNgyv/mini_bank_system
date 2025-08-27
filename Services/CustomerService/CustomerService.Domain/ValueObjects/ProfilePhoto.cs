namespace CustomerService.Domain.ValueObjects;

public class ProfilePhoto
{
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public long FileSize { get; private set; }
    public string ContentType { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public string InternalUrl { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public ProfilePhoto(string fileName, string filePath, long fileSize, string contentType, int width, int height)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty", nameof(fileName));
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty", nameof(filePath));
        if (fileSize <= 0)
            throw new ArgumentException("File size must be positive", nameof(fileSize));
        if (width <= 0 || height <= 0)
            throw new ArgumentException("Image dimensions must be positive", nameof(width));

        ValidateImageType(contentType);
        ValidateFileSize(fileSize);

        FileName = fileName;
        FilePath = filePath;
        FileSize = fileSize;
        ContentType = contentType.ToLowerInvariant();
        Width = width;
        Height = height;
        InternalUrl = GenerateInternalUrl(fileName);
        UploadedAt = DateTime.UtcNow;
    }

    private static void ValidateImageType(string contentType)
    {
        var allowedTypes = new[] 
        { 
            "image/jpeg", 
            "image/jpg", 
            "image/png", 
            "image/gif",
            "image/webp"
        };
        
        if (!allowedTypes.Contains(contentType.ToLowerInvariant()))
            throw new ArgumentException($"Invalid file type. Allowed types: {string.Join(", ", allowedTypes)}", nameof(contentType));
    }

    private static void ValidateFileSize(long fileSize)
    {
        const long maxSizeBytes = 5 * 1024 * 1024; // 5MB
        if (fileSize > maxSizeBytes)
            throw new ArgumentException($"File size too large. Maximum allowed: {maxSizeBytes / (1024 * 1024)}MB", nameof(fileSize));
    }

    private static string GenerateInternalUrl(string fileName)
    {
        var fileId = Guid.NewGuid().ToString("N")[..8];
        var extension = Path.GetExtension(fileName);
        return $"/api/v1/customers/photos/{fileId}{extension}";
    }

    // Photo metadata extraction methods (PDF requirement)
    public string GetFileExtension() => Path.GetExtension(FileName);
    public double GetFileSizeInMB() => Math.Round(FileSize / (1024.0 * 1024.0), 2);
    public string GetImageResolution() => $"{Width}x{Height}";
    public double GetAspectRatio() => Math.Round((double)Width / Height, 2);
    
    public bool IsPortrait() => Height > Width;
    public bool IsLandscape() => Width > Height;
    public bool IsSquare() => Width == Height;
    public bool IsHighResolution() => Width >= 1920 || Height >= 1920;

    public override string ToString() => $"{FileName} ({GetFileSizeInMB()}MB, {GetImageResolution()})";
    
    protected bool Equals(ProfilePhoto other) => 
        FileName == other.FileName && 
        FilePath == other.FilePath && 
        FileSize == other.FileSize;
        
    public override bool Equals(object? obj) => obj is ProfilePhoto other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(FileName, FilePath, FileSize);
}