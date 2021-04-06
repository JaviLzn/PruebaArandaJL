using System;

namespace Domain.Entities
{
    public class Audit
    {
        public int Id { get; set; }
        public string Info { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
