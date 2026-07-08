using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Categories;

namespace WebApi.Infrastructure.Persistence.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id).HasName("pk_categories");

        builder.Property(c => c.Id).HasColumnName("id").IsRequired();

        builder
            .Property(c => c.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.HasIndex(c => c.Name).IsUnique().HasDatabaseName("uq_categories_name");

        builder
            .Property(c => c.Description)
            .HasColumnName("description")
            .HasColumnType("varchar(500)");

        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
    }
}
