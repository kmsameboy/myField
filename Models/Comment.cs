using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Comment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Well Well { get; set; }

        public string Author { get; set; }

        [Required]
        public DateTime PostedDate { get; set; }

        [Required]
        public string Body { get; set; }

    }
}
