using DaraSurvey.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaraSurvey.Entities
{
    public class Widget : EntityBase<int>
    {
        public string WidgetData { get; set; }
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
