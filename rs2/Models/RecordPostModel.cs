using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using rs2.Models.Database;

namespace rs2.Models
{
    public class RecordPostModel
    {
        public float Bx { get; set; } = 0;
        public float By { get; set; } = 0;
        public float Ax { get; set; } = 0;
        public float Ay { get; set; } = 0;

        public Record toRecord(User owner)
        {
            return new Record()
            {
                User = owner,
                BeforeX = Bx,
                BeforeY = By,
                AfterX = Ax,
                AfterY = Ay
            };
        }
    }
}
