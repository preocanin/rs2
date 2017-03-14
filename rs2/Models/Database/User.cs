using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rs2.Models.Database
{
    public enum Role { Admin, Client }
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId {get;set;}

        [Column(TypeName = "character varying(255)")]
        public string Username { get; set; }

        /*
        [Column(TypeName = "character varying(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "character varying(50)")]
        public string LastName { get; set; }
        */

        [Key]
        [Column(TypeName = "character varying(255)")]
        public string Email { get; set; }

        public Role Role { get; set; }

        [Column(TypeName = "character varying(255)")]
        public string Password { get; set; }

        [Column(TypeName = "character varying(255)")]
        public string Salt { get; set; }
        

        public virtual ICollection<Record> Records { get; set; }

    }
}
