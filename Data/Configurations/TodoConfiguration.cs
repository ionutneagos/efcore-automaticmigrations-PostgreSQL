using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EFCore.AutomaticMigrations.EF.Sample;

namespace EFCore.AutomaticMigrations.Sample.Data.Configurations
{
    internal sealed class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable("Todos");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).UseIdentityAlwaysColumn();
            builder.Property(t => t.Title).HasMaxLength(400).IsRequired();
         
            //map other properties here
        }
    }
}
