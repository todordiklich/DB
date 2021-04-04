using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectExportViewModel
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public TaskExportViewModel[] Tasks { get; set; }
    }

    [XmlType("Task")]
    public class TaskExportViewModel
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Label")]
        public string Label { get; set; }
    }
}
//<Project TasksCount="10">
//    <ProjectName>Hyster-Yale</ProjectName>
//    <HasEndDate>No</HasEndDate>
//    <Tasks>
//      <Task>
//        <Name>Guadeloupe</Name>
//        <Label>JavaAdvanced</Label>
//      </Task>
//      <Task>
//        <Name>Longbract Pohlia Moss</Name>
//        <Label>EntityFramework</Label>
//      </Task>
//     ......
//      <Task>
//        <Name>Pacific</Name>
//        <Label>Priority</Label>
//      </Task>
//    </Tasks>
//  </Project>
