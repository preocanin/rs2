using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rs2.Models
{
    public class RecordPutModel
    {
        public int RecordId { get; set; }
        public float Bx { get; set; } = 0;
        public float By { get; set; } = 0;
        public float Ax { get; set; } = 0;
        public float Ay { get; set; } = 0;
    }
}
