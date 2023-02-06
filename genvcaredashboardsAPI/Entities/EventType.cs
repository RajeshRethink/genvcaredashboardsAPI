using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace genvcaredashboardsAPI.Entities
{
    public partial class EventType
    {
        //public EventType()
        //{
        //    PatientEvents = new HashSet<PatientEvents>();
        //}

        [Key]
        public int EventTypeId { get; set; }
        [StringLength(50)]
        public string EventTypeName { get; set; }
        [StringLength(50)]
        public string Description { get; set; }

        //[InverseProperty("EventType")]
        //public virtual ICollection<PatientEvents> PatientEvents { get; set; }
    }
}
