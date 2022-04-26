using DaraSurvey.Core;
using DaraSurvey.WidgetServices;
using DaraSurvey.WidgetServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaraSurvey.Services.SurveryServices.Entities
{
    public class Question : EntityBase<int>
    {
        private readonly IWidgetService _widgetService;

        public Question()
        {
            var serviceProvider = Startup.ServiceCollection.BuildServiceProvider();
            _widgetService = serviceProvider.GetRequiredService<IWidgetService>();
        }

        public string Text { get; set; }

        public string WidgetData { get; set; }

        [ForeignKey("Survey")]
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

        public bool IsRequired { get; set; }

        public bool IsCountable { get; set; }

        public ViewModelBase Widget
        {
            get { return _widgetService.GetWidget(this.WidgetData); }
        }
    }

    // ----------------------

    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder
                .ToTable("Questions")
                .HasQueryFilter(o => !o.Deleted.HasValue);

            builder
                .HasOne(q => q.Survey)
                .WithMany(s => s.Questions);
        }
    }
}
