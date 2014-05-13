using System;
using System.Collections.Generic;
using System.Text;

namespace CustomUpdater.DataTypes.Enums
{
  /// <summary>
  /// Enumeration of the result of an update
  /// </summary>
  public enum EUpdateResult
  {
    /// <summary>
    /// the Update is success
    /// </summary>
    Success = 0,

    /// <summary>
    /// The settings-file is not correct
    /// </summary>
    SettingsAreIncorect = 1,

    /// <summary>
    /// The web current version cant read, maybe the site is not available
    /// </summary>
    WebCurrentVersionCantRead = 2,

    /// <summary>
    /// no new version available
    /// </summary>
    NoNewVersionAvailable = 3,

    /// <summary>
    /// The web data cant download, maybe the site is not available
    /// </summary>
    WebDatasCantDownload = 4,

    /// <summary>
    /// general undefined error
    /// </summary>
    Error = 100,

    None = 101,
  }

}
