using DaraSurvey.Entities;
using DaraSurvey.Interfaces;
using DaraSurvey.Services.SurveryServices.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace DaraSurvey.Core
{
    public class DB : IdentityDbContext<User>
    {
        public DB(DbContextOptions<DB> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // ------------------

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .ApplyConfiguration(new ProfileConfiguration())
                .ApplyConfiguration(new QuestionConfiguration())
                .ApplyConfiguration(new QuestionResponseConfiguration())
                .ApplyConfiguration(new SurveyConfiguration())
                .ApplyConfiguration(new UsersSurveyConfiguration());
        }

        // --------------------

        public override int SaveChanges()
        {
            IFileSystemService fileService = null;

            var entityGroups = ChangeTracker.Entries().GroupBy(o => o.Entity).ToList();

            entityGroups.ForEach(g =>
            {
                var attachments = AttachmentAttributeInfo.GetAttachmentAttributes(g.Key.GetType()).ToArray();
                if (attachments.Length == 0) return;

                var changedEntries = g.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

                foreach (var attachment in attachments)
                {
                    foreach (var entry in changedEntries)
                    {
                        var serviceProvider = DaraSurvey.Core.Startup.ServiceCollection.BuildServiceProvider();
                        fileService = serviceProvider.GetService<IFileSystemService>();

                        var oldFilePaths = entry.GetDatabaseValues()?.GetValue<string>(attachment.Property.Name);

                        if (entry.State == EntityState.Deleted)
                            fileService.DeleteFiles(oldFilePaths);
                        else
                        {
                            var propertyEntry = entry.Property(attachment.Property.Name);

                            var newFilePaths = (string)propertyEntry.CurrentValue;
                            var persistedFilePaths = fileService.ManipulateAttachments(oldFilePaths, newFilePaths, attachment.Container, attachment.Bucket);

                            attachment.Property.SetValue(entry.Entity, persistedFilePaths);
                        }
                    }
                }
            });

            return base.SaveChanges();
        }
    }
}
