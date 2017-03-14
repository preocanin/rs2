using System.ComponentModel.DataAnnotations;

namespace rs2.Models.Database
{
    public class Record
    {
        [Key]
        public int RecordId { get; set; }

        public User User { get; set; }
    }
}
