namespace CustomUpdater.DataTypes.Objects
{
  using System;
  using System.Xml.Serialization;

  [Serializable(), XmlRoot("Settings", Namespace = "")]
  public class Settings
  {
    [XmlElement("ApplicationPath")]
    public string ApplicationPath { get; set; }

    [XmlElement("CurrentVersion")]
    public string CurrentVersion { get; set; }

    [XmlElement("TempUpdateFilePath")]
    public string TempUpdateFilePath { get; set; }

    [XmlElement("WebUpdateFileName")]
    public string WebUpdateFileName { get; set; }

    [XmlElement("WebUpdateFilePath")]
    public string WebUpdateFilePath { get; set; }

    [XmlElement("WebUpdateVersionPath")]
    public string WebUpdateVersionPath { get; set; }
  }
}