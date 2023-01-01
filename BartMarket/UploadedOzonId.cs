using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BartMarket
{
    public class UploadedOzonId
    {
        [Key]
        public int Id { get; set; }
        public string OzonId { get; set; }
    }
}
