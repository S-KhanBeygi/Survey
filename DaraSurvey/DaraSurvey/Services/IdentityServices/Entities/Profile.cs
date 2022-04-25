using DaraSurvey.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaraSurvey.Entities
{
    public class Profile : EntityBase<string>
    {
        [ForeignKey("Id")]
        public User User { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        [Attachment("Users", "Image")]
        public string Image { get; set; }

        public string NationalCode { get; set; }
    }

    // --------------------

    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder
                .ToTable("Profiles")
                .HasQueryFilter(o => !o.Deleted.HasValue);
        }
    }

    // --------------------

    public enum Gender
    {
        female = 0,
        male = 1
    }
}
