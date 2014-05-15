namespace CustomUpdater.DataTypes.Interfaces
{
  using CustomUpdater.DataTypes.Enums;
  using CustomUpdater.DataTypes.Objects;

  public interface IUpdaterBusiness
  {
    /// <summary>
    /// Loads the settings.
    /// </summary>
    /// <param name="xmlSettingsPath">The XML settings path.</param>
    /// <returns></returns>
    Settings LoadSettings(string xmlSettingsPath);

    /// <summary>
    /// Updates the specified updater settings.
    /// </summary>
    /// <param name="updaterSettings">The updater settings.</param>
    /// <param name="processID">The process identifier.</param>
    /// <returns></returns>
    EUpdateResult Update(Settings updaterSettings, int processID);
  }
}