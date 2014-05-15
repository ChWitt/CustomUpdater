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
    None = 0,

    /// <summary>
    /// the Update is success
    /// </summary>
    Success = 1,

    /// <summary>
    /// The settings-file is not correct
    /// </summary>
    SettingsAreIncorect = 2,

    /// <summary>
    /// The web current version cant read, maybe the site is not available
    /// </summary>
    WebCurrentVersionCantRead = 3,

    /// <summary>
    /// no new version available
    /// </summary>
    NoNewVersionAvailable = 4,

    /// <summary>
    /// The web data cant download, maybe the site is not available
    /// </summary>
    WebDatasCantDownload = 5,

    /// <summary>
    /// general undefined error
    /// </summary>
    Error = 100,

  }

}
