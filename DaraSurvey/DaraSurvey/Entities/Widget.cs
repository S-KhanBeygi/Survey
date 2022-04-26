using DaraSurvey.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DaraSurvey.Entities
{
    public class Widget : EntityBase<int>
    {
        public string Data { get; set; }
    }

    // ----------------------

    public class WidgetConfiguration : IEntityTypeConfiguration<Widget>
    {
        public void Configure(EntityTypeBuilder<Widget> builder)
        {
            builder
                .ToTable("Widget")
                .HasQueryFilter(o => !o.Deleted.HasValue);
        }
    }
}
