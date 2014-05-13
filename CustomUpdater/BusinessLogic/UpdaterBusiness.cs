using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CustomUpdater.DataTypes.Enums;
using CustomUpdater.DataTypes.Interfaces;
using CustomUpdater.DataTypes.Objects;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System.Windows.Forms;

namespace CustomUpdater.BusinessLogic
{
  public class UpdaterBusiness : IUpdaterBusiness
  {
    public Settings LoadSettings(string xmlSettingsPath)
    {
      Settings updaterSettings;
      XmlSerializer serializer = new XmlSerializer(typeof(Settings));

      using (FileStream fileStream = new FileStream(xmlSettingsPath, FileMode.Open))
      {
        using (XmlReader reader = XmlReader.Create(fileStream))
        {
          updaterSettings = (Settings)serializer.Deserialize(reader);
          reader.Close();
        }

        fileStream.Close();
      }

      return updaterSettings;
    }

    public EUpdateResult Update(Settings updaterSettings)
    {
      Version currentVersion;      
      EUpdateResult updateResult = EUpdateResult.None;

      if (!this.AreSettingsValid(updaterSettings, out currentVersion))
      {
        return EUpdateResult.SettingsAreIncorect;
      }

      updateResult = this.IsNewVersionAvailable(updaterSettings.WebUpdateVersionPath, currentVersion);
      if (updateResult != EUpdateResult.None)
      {
        return updateResult;
      }

      updateResult = this.DownloadDataFiles(updaterSettings);
      if (updateResult != EUpdateResult.None)
      {
        return updateResult;
      }

      return EUpdateResult.Success;
    }

    private bool AreSettingsValid(Settings updaterSettings, out Version currentVersion)
    {
      if (!Version.TryParse(updaterSettings.CurrentVersion, out currentVersion))
      {
        return false;
      }

      string websiteRegEx = "^http(s)?://([\\w-]+.)+[\\w-]+(/[\\w- ./?%&=])?$";
      Regex testingSettings = new Regex(websiteRegEx);

      if (!testingSettings.IsMatch(updaterSettings.WebUpdateFilePath) || !testingSettings.IsMatch(updaterSettings.WebUpdateVersionPath))
      {
        return false;
      }

      if (updaterSettings.TempUpdateFilePath.Length < 1 || 
        updaterSettings.ApplicationPath.Length < 1 ||
        System.IO.Directory.Exists(updaterSettings.ApplicationPath) ||
        System.IO.Directory.Exists(updaterSettings.TempUpdateFilePath))
      {
        return false;
      }

      return true;
    }

    private EUpdateResult IsNewVersionAvailable(string webUpdateVersionPath, Version currentVersion)
    {
      string webVersionString;
      Version webCurrentVerion;

      try
      {
        webVersionString = new System.Net.WebClient().DownloadString(webUpdateVersionPath);
      }
      catch
      {
        return EUpdateResult.WebCurrentVersionCantRead;
      }

      if (Version.TryParse(webVersionString, out webCurrentVerion) && 
          webCurrentVerion.CompareTo(currentVersion) >= 1)
      {
        return EUpdateResult.None;
      }
      else
      {
        return EUpdateResult.NoNewVersionAvailable;
      }
    }

    private EUpdateResult DownloadDataFiles(Settings updaterSettings, out MemoryStream dataStream)
    {
      byte[] data = null;
      try
      {
        data = new System.Net.WebClient().DownloadData(updaterSettings.WebUpdateFilePath + updaterSettings.WebUpdateFileName);
      }
      catch
      {
        dataStream = null;
        return EUpdateResult.WebDatasCantDownload;
      }

      dataStream = new MemoryStream(data);

      return EUpdateResult.None;
    }

    private EUpdateResult ReplaceFiles(Settings updaterSettings, MemoryStream dataStream)
    {
      this.RenameOldFiles(updaterSettings.ApplicationPath);

      FastZip fastZip = new FastZip();
      fastZip.ExtractZip(dataStream, updaterSettings.ApplicationPath, FastZip.Overwrite.Always, null, string.Empty, string.Empty, false, true);

      return EUpdateResult.None;
    }

    private void RenameOldFiles(string path)
    {
      foreach (string fileName in System.IO.Directory.GetFiles(path))
      {
        System.IO.File.Move(path + fileName, path + fileName + ".temp");
      }

      foreach (string dictionaryName in System.IO.Directory.GetDirectories(path))
      {
        RenameOldFiles(path + dictionaryName);
      }
    }

    private void DeleteOldFiles(string path)
    {

    }

  }
}