using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsedBookExchange.Domain.Entities;

namespace UsedBookExchange.Infrastructure.Configurations;

public class BookRequestConfiguration
	: IEntityTypeConfiguration<BookRequest>
{
	public void Configure(EntityTypeBuilder<BookRequest> builder)
	{
		builder.HasKey(request => request.Id);

		builder.Property(request => request.RequesterId)
			.IsRequired()
			.HasMaxLength(450);

		builder.Property(request => request.Message)
			.HasMaxLength(500);

		builder.Property(request => request.Status)
			.HasConversion<string>();

		builder.Property(request => request.CreatedAt)
			.IsRequired();

		builder.HasOne(request => request.Book)
			.WithMany()
			.HasForeignKey(request => request.BookId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(request => new
		{
			request.BookId,
			request.RequesterId
		});
	}
}