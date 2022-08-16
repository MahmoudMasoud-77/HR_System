using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models
{
    public class Holidays
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{DD/MM/YYYY}")]
        public DateTime Date { get; set; }
    }
}
