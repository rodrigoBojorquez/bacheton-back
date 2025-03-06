using Bacheton.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bacheton.Infrastructure.Data.Configurations;

public class ReportConfig : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        var reportStatusConverter = new EnumToStringConverter<ReportStatus>();
        var reportSeverityConverter = new EnumToStringConverter<ReportSeverity>();

        builder.Property(r => r.Severity)
            .HasConversion(reportSeverityConverter);

        builder.Property(r => r.Status)
            .HasConversion(reportStatusConverter);
        
        builder.HasOne(r => r.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ResolvedBy)
            .WithMany(u => u.ResolvedReports)
            .HasForeignKey(r => r.ResolvedById)
            .OnDelete(DeleteBehavior.SetNull);
    }
}