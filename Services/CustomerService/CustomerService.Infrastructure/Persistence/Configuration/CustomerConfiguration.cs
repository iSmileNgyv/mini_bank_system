using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Infrastructure.Persistence.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Table configuration
        builder.ToTable("CUSTOMERS");
        
        // Primary key
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        // Customer Number - Unique
        builder.Property(x => x.CustomerNumber)
            .HasColumnName("CUSTOMER_NUMBER")
            .HasMaxLength(20)
            .IsRequired();
        builder.HasIndex(x => x.CustomerNumber)
            .IsUnique()
            .HasDatabaseName("IX_CUSTOMERS_CUSTOMER_NUMBER");

        // Personal information
        builder.Property(x => x.FirstName)
            .HasColumnName("FIRST_NAME")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasColumnName("LAST_NAME")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.DateOfBirth)
            .HasColumnName("DATE_OF_BIRTH")
            .IsRequired();

        // Email Value Object
        builder.OwnsOne(x => x.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("EMAIL")
                .HasMaxLength(320)
                .IsRequired();
                
            // Index on email value
            email.HasIndex(e => e.Value)
                .IsUnique()
                .HasDatabaseName("IX_CUSTOMERS_EMAIL");
        });

        // PhoneNumber Value Object
        builder.OwnsOne(x => x.PhoneNumber, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("PHONE_NUMBER")
                .HasMaxLength(20)
                .IsRequired();
        });

        // Address Value Object
        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.Street)
                .HasColumnName("STREET")
                .HasMaxLength(500)
                .IsRequired();
                
            address.Property(a => a.City)
                .HasColumnName("CITY")
                .HasMaxLength(100)
                .IsRequired();
                
            address.Property(a => a.Country)
                .HasColumnName("COUNTRY")
                .HasMaxLength(100)
                .IsRequired();
                
            address.Property(a => a.PostalCode)
                .HasColumnName("POSTAL_CODE")
                .HasMaxLength(20);
                
            address.Property(a => a.State)
                .HasColumnName("STATE")
                .HasMaxLength(100);
        });

        // ProfilePhoto Value Object (nullable)
        builder.OwnsOne(x => x.ProfilePhoto, photo =>
        {
            photo.Property(p => p.FileName)
                .HasColumnName("PHOTO_FILE_NAME")
                .HasMaxLength(255);
                
            photo.Property(p => p.FilePath)
                .HasColumnName("PHOTO_FILE_PATH")
                .HasMaxLength(1000);
                
            photo.Property(p => p.FileSize)
                .HasColumnName("PHOTO_FILE_SIZE");
                
            photo.Property(p => p.ContentType)
                .HasColumnName("PHOTO_CONTENT_TYPE")
                .HasMaxLength(100);
                
            photo.Property(p => p.Width)
                .HasColumnName("PHOTO_WIDTH");
                
            photo.Property(p => p.Height)
                .HasColumnName("PHOTO_HEIGHT");
                
            photo.Property(p => p.InternalUrl)
                .HasColumnName("PHOTO_INTERNAL_URL")
                .HasMaxLength(500);
                
            photo.Property(p => p.UploadedAt)
                .HasColumnName("PHOTO_UPLOADED_AT");
        });

        // Enums
        builder.Property(x => x.CustomerType)
            .HasColumnName("CUSTOMER_TYPE")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.AccountStatus)
            .HasColumnName("ACCOUNT_STATUS")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.KycStatus)
            .HasColumnName("KYC_STATUS")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.RiskLevel)
            .HasColumnName("RISK_LEVEL")
            .HasConversion<int>()
            .IsRequired();

        // Optional fields
        builder.Property(x => x.BranchCode)
            .HasColumnName("BRANCH_CODE")
            .HasMaxLength(10);

        builder.Property(x => x.RelationshipManagerId)
            .HasColumnName("RELATIONSHIP_MANAGER_ID");

        // Timestamps
        builder.Property(x => x.CreatedDate)
            .HasColumnName("CREATED_DATE")
            .IsRequired();

        builder.Property(x => x.UpdatedDate)
            .HasColumnName("UPDATED_DATE");

        // Indexes for performance
        builder.HasIndex(x => x.AccountStatus)
            .HasDatabaseName("IX_CUSTOMERS_ACCOUNT_STATUS");
            
        builder.HasIndex(x => x.KycStatus)
            .HasDatabaseName("IX_CUSTOMERS_KYC_STATUS");
            
        builder.HasIndex(x => x.BranchCode)
            .HasDatabaseName("IX_CUSTOMERS_BRANCH_CODE");
            
        builder.HasIndex(x => x.CreatedDate)
            .HasDatabaseName("IX_CUSTOMERS_CREATED_DATE");
    }
}