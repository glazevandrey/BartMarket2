using System.ComponentModel.DataAnnotations;

namespace BartMarket
{
    public class WarehouseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
