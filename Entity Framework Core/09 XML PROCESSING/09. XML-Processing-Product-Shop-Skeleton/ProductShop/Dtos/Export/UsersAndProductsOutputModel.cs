using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Users")]
    public class UsersAndProductsOutputModel
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UserOutputModel[] Users { get; set; }
    }
}