using System.Xml.Serialization;

namespace CarDealer.DTO.Output
{
    [XmlType("customer")]
    public class CustomerOutputModel
    {
        [XmlAttribute("full-name")]
        public string FullName { get; set; }

        [XmlAttribute("bought-cars")]
        public int CarsBought { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }
    }
}