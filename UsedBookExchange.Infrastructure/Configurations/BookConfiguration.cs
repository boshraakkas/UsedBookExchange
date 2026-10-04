using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsedBookExchange.Domain.Entities;

namespace UsedBookExchange.Infrastructure.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
	public void Configure(EntityTypeBuilder<Book> builder)
	{
		builder.HasKey(book => book.Id);

		builder.Property(book => book.Title)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(book => book.Author)
			.IsRequired()
			.HasMaxLength(150);

		builder.Property(book => book.Description)
			.HasMaxLength(1000);

		builder.Property(book => book.Category)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(book => book.OwnerId)
			.IsRequired()
			.HasMaxLength(450);

		builder.Property(book => book.ImageUrl)
			.HasMaxLength(500);

		builder.Property(book => book.Status)
			.HasConversion<string>();

		builder.Property(book => book.Condition)
			.HasConversion<string>();

		builder.Property(book => book.CreatedAt)
			.IsRequired();
	}
}