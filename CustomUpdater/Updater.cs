using System;
using System.Collections.Generic;
using System.Text;
using CustomUpdater.DataTypes.Objects;
using CustomUpdater.DataTypes.Interfaces;
using CustomUpdater.BusinessLogic;
using CustomUpdater.DataTypes.Enums;

namespace CustomUpdater
{
    public class Updater 
    {
      IUpdaterBusiness businessLogic;
      Settings updaterSettings;

      public Updater(string settingsXMLPath)
      {
        this.businessLogic = new UpdaterBusiness();
 
        if (settingsXMLPath.Length < 1 || System.IO.File.Exists(settingsXMLPath))
        {
          this.updaterSettings = this.businessLogic.LoadSettings(settingsXMLPath);
        }
      }

      public EUpdateResult Update()
      {
        return this.businessLogic.Update(this.updaterSettings);
      }
    }

}
