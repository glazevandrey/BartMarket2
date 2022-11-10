using System.ComponentModel.DataAnnotations;

namespace BartMarket
{
    public class WarehouseSetting
    {
        [Key]
        public int Id { get; set; }
        public string Filter { get; set; }
        public int WarehouseId { get; set; }
    }
}
