using System.Windows.Forms;
using CustomUpdater.BusinessLogic;
using CustomUpdater.DataTypes.Enums;
using CustomUpdater.DataTypes.Interfaces;
using CustomUpdater.DataTypes.Objects;

namespace CustomerUpdater.UI
{
  public partial class MainForm : Form
  {
    private IUpdaterBusiness businessLogic;
    private Settings updaterSettings;

    public MainForm()
    {
      InitializeComponent();
    }

    public void InitUpdater(string settingsXMLPath)
    {
      this.businessLogic = new UpdaterBusiness();

      if (settingsXMLPath.Length < 1 || System.IO.File.Exists(settingsXMLPath))
      {
        this.updaterSettings = this.businessLogic.LoadSettings(settingsXMLPath);
      }


    }

    private void MainForm_Load(object sender, System.EventArgs e)
    {
      EUpdateResult updateResult = businessLogic.Update(this.updaterSettings);
      
      switch (updateResult)
      {
        case EUpdateResult.Success:
          break;
          
        case EUpdateResult.NoNewVersionAvailable:
          break;
          
        case EUpdateResult.SettingsAreIncorect:
          break;

        case EUpdateResult.WebCurrentVersionCantRead:
          break;

        case EUpdateResult.WebDatasCantDownload:
          break;
           
        case EUpdateResult.Error:
          break;
      }
    }
  }
}