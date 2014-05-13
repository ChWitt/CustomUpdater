using System;
using System.Collections.Generic;
using System.Text;
using CustomUpdater.DataTypes.Objects;
using CustomUpdater.DataTypes.Enums;

namespace CustomUpdater.DataTypes.Interfaces
{
  public interface IUpdaterBusiness
  {
    Settings LoadSettings(string xmlSettingsPath);

    EUpdateResult Update(Settings updaterSettings);
  }
}
