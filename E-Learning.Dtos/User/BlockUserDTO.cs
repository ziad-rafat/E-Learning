using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Dtos.User
{
    [NotMapped]
    public class BlockUserDTO
    {
        [Required(ErrorMessage = "Field can't be empty")]
        public string Id { get; set; }
        [Required]
        public bool IsBlocked { get; set; }
    }
}
