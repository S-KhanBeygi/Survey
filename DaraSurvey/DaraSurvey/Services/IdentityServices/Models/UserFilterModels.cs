using DaraSurvey.Core;
using DaraSurvey.Entities;
using System;

namespace DaraSurvey.Models
{
    public class UserFilter : FilterBase, IFilterable
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? MinBirthDate { get; set; }
        public DateTime? MaxBirthDate { get; set; }

        public bool? HasImage { get; set; }

        public string NationalCode { get; set; }
    }

    // --------------------

    public class UserOrderedFilter : UserFilter, IOrderedFilterable
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Sort { get; set; }

        public bool Asc { get; set; }

        public bool RndArgmnt { get; set; }
    }
}
