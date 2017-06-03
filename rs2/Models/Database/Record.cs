using System.ComponentModel.DataAnnotations;

namespace rs2.Models.Database
{
    public class Record
    {
        [Key]
        public int RecordId { get; set; }

        // X and Y coordinates of vectors before drops
        public float BeforeX { get; set; }

        public float BeforeY { get; set; }

        // X and Y coordinates of vectors after drops
        public float AfterX { get; set; }

        public float AfterY { get; set; }

        public User User { get; set; }
    }
}
