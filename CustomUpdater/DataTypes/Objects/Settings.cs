using System;
using System.Xml.Serialization;

namespace CustomUpdater.DataTypes.Objects
{
  [Serializable(), XmlRoot("Settings", Namespace="")]
  public class Settings
  {
    [XmlElement("ApplicationPath")]
    public string ApplicationPath { get; set; }

    [XmlElement("TempUpdateFilePath")]
    public string TempUpdateFilePath { get; set; }

    [XmlElement("WebUpdateVersionPath")]
    public string WebUpdateVersionPath { get; set; }

    [XmlElement("WebUpdateFilePath")]
    public string WebUpdateFilePath { get; set; }

    [XmlElement("CurrentVersion")]
    public string CurrentVersion { get; set; }

    [XmlElement("WebUpdateFileName")]
    public string WebUpdateFileName { get; set; }

    [XmlElement("ExeName")]
    public string ExeName { get; set; }
  }
}