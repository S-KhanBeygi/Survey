using DaraSurvey.Core;
using DaraSurvey.Entities;
using DaraSurvey.WidgetServices;
using DaraSurvey.WidgetServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaraSurvey.Services.SurveryServices.Entities
{
    public class Survey : EntityBase<int>
    {
        private readonly IWidgetService _widgetService;

        public Survey()
        {
            var serviceProvider = Startup.ServiceCollection.BuildServiceProvider();
            _widgetService = serviceProvider.GetRequiredService<IWidgetService>();
        }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? Published { get; set; }

        public DateTime? Expired { get; set; }

        public DateTime? ExamStart { get; set; }

        public TimeSpan? Duration { get; set; }

        public TimeSpan? AllowedDelayTime { get; set; }

        [ForeignKey("SurveyDesigner")]
        public string SurveyDesignerId { get; set; }
        public User SurveyDesigner { get; set; }

        [Attachment("Survey", "Logo")]
        public string Logo { get; set; }

        public string WelcomePageWidgetData { get; set; }

        public string ThankYouPageWidgetData { get; set; }

        public IEnumerable<Question> Questions { get; set; }

        public IEnumerable<UsersSurvey> UsersSurvey { get; set; }


        public ViewModelBase WelcomePageWidget
        {
            get { return _widgetService.GetWidget(this.WelcomePageWidgetData); }
        }

        public ViewModelBase ThankYouPageWidget
        {
            get { return _widgetService.GetWidget(this.ThankYouPageWidgetData); }
        }
    }

    // --------------------

    public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
    {
        public void Configure(EntityTypeBuilder<Survey> builder)
        {
            builder
                .ToTable("Surveys")
                .HasQueryFilter(o => !o.Deleted.HasValue);
        }
    }
}
