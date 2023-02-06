using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace genvcaredashboardsAPI.Models.Event
{
    public class AllUniqueEventsAndTestsDTO
    {
        public int RnO { get; set; }
        public int PatientId { get; set; }
        public int EventId { get; set; }
        public string TestName { get; set; }
        public string ModalityValue { get; set; }
        public string TestResults { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
