using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("User")]
    public class UserExportViewModel
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlArray("Purchases")]
        public PurchaseExportViewModel[] Purchases { get; set; }

        [XmlElement("TotalSpent")]
        public decimal TotalSpent { get; set; }
    }
}
//<User username="mgraveson">
//    <Purchases>
//      <Purchase>
//        <Card>7991 7779 5123 9211</Card>
//        <Cvc>340</Cvc>
//        <Date>2017-08-31 17:09</Date>
//        <Game title="Counter-Strike: Global Offensive">
//          <Genre>Action</Genre>
//          <Price>12.49</Price>
//        </Game>
//      </Purchase>
//      ...
//    </Purchases>
//    <TotalSpent>72.48</TotalSpent>
//  </User>