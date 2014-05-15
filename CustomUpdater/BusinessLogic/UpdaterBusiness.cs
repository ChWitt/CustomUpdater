namespace CustomUpdater.BusinessLogic
{
  using System;
  using System.IO;
  using System.Text.RegularExpressions;
  using System.Xml;
  using System.Xml.Serialization;
  using CustomUpdater.DataTypes.Enums;
  using CustomUpdater.DataTypes.Interfaces;
  using CustomUpdater.DataTypes.Objects;
  using ICSharpCode.SharpZipLib.Zip;

  /// <summary>
  /// The business logic of the updater
  /// </summary>
  public class UpdaterBusiness : IUpdaterBusiness
  {
    /// <summary>
    /// Loads the settings.
    /// </summary>
    /// <param name="xmlSettingsPath">The XML settings path.</param>
    /// <returns>returns a settings-object with the values of the XML file</returns>
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

    /// <summary>
    /// Updates the specified updater settings.
    /// </summary>
    /// <param name="updaterSettings">The updater settings.</param>
    /// <returns>the result of the update</returns>
    public EUpdateResult Update(Settings updaterSettings, int processID)
    {
      EUpdateResult updateResult;

      // TODO get admin rights
      try
      {
        MemoryStream dataStream = null;
        updateResult = this.CheckUpdatePossible(updaterSettings, out dataStream);
        if (updateResult == EUpdateResult.None)
        {
          System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(processID);
          process.Kill();

          try
          {
            this.BackUpOldData();
            updateResult = this.UpdateDatabase();
            updateResult = this.UpdateFileData(updaterSettings, dataStream);
          }
          catch
          {
            updateResult = EUpdateResult.Error;
          }

          if (updateResult != EUpdateResult.Success)
          {
            this.RestoreBackup();
          }

          // TODO Save Settings with the new version

          System.Diagnostics.Process.Start(process.MainModule.FileName);
        }
      }
      catch (Exception e)
      {
        updateResult = EUpdateResult.Error;
        // TODO Error schreiben
      }

      return updateResult;
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

    private void BackUpOldData()
    {
      // TODO implement the function
      throw new NotImplementedException();
    }

    private EUpdateResult CheckUpdatePossible(Settings updaterSettings, out MemoryStream dataStream)
    {
      EUpdateResult updateResult = EUpdateResult.None;
      Version currentVersion;

      if (!this.AreSettingsValid(updaterSettings, out currentVersion))
      {
        dataStream = null;
        return EUpdateResult.SettingsAreIncorect;
      }

      updateResult = this.IsNewVersionAvailable(updaterSettings.WebUpdateVersionPath, currentVersion);
      if (updateResult != EUpdateResult.None)
      {
        dataStream = null;
        return updateResult;
      }

      updateResult = this.DownloadDataFiles(updaterSettings, out dataStream);
      if (updateResult != EUpdateResult.None)
      {
        return updateResult;
      }

      return EUpdateResult.None;
    }

    private void DeleteFiles(string directoryPath)
    {
      foreach (string subDirectory in Directory.GetDirectories(directoryPath))
      {
        if (subDirectory.EndsWith("UpdateBackUp"))
        {
          this.DeleteFiles(subDirectory);
        }
      }

      foreach (string fileName in Directory.GetFiles(directoryPath))
      {
        File.Delete(fileName);
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

    private void RestoreBackup()
    {
      // TODO implement the function
      throw new NotImplementedException();
    }

    private EUpdateResult UpdateDatabase()
    {
      // TODO implement the function
      return EUpdateResult.Success;
    }

    private EUpdateResult UpdateFileData(Settings updaterSettings, MemoryStream dataStream)
    {
      this.DeleteFiles(updaterSettings.ApplicationPath);

      FastZip fastZip = new FastZip();
      fastZip.ExtractZip(dataStream, updaterSettings.ApplicationPath, FastZip.Overwrite.Always, null, string.Empty, string.Empty, false, true);

      return EUpdateResult.Success;
    }
  }
}