using System.Xml.Serialization;

namespace CarDealer.DTO.Inport
{
    [XmlType("partId")]
    public class PartCarInputModel
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}