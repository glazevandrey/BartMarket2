using System.ComponentModel.DataAnnotations;

namespace BartMarket
{
    public class LinkModel
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
    }
}
