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
    private int processID;

    public MainForm(string settingsXMLPath, int processID)
    {
      InitializeComponent();

      this.businessLogic = new UpdaterBusiness();

      if (settingsXMLPath.Length < 1 || System.IO.File.Exists(settingsXMLPath))
      {
        this.updaterSettings = this.businessLogic.LoadSettings(settingsXMLPath);
      }

      this.processID = processID;
    }

    private void MainForm_Load(object sender, System.EventArgs e)
    {
      EUpdateResult updateResult = businessLogic.Update(this.updaterSettings, this.processID);
      
      switch (updateResult)
      {
        case EUpdateResult.Success:
          MessageBox.Show(this, "Update war erfolgreich.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
          this.Close();
          break;
          
        case EUpdateResult.NoNewVersionAvailable:
          MessageBox.Show(this, "Sie haben die aktuelle Version installiert", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
          this.Close();
          break;
          
        case EUpdateResult.SettingsAreIncorect:
          MessageBox.Show(this, "Die übergebenen Einstellungen sind nicht korrekt. Wenden Sie sich an den Adminstrator.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
          this.Close();
          break;

        case EUpdateResult.WebCurrentVersionCantRead:
          MessageBox.Show(this, "Es konnte nicht die Version geprüft werden. Versuchen Sie es zu einem späteren Zeitpunkt wieder", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
          break;

        case EUpdateResult.WebDatasCantDownload:
          MessageBox.Show(this, "Es konnten nicht die Daten zum aktualisieren heruntergeladen werden. Versuchen Sie es zu einem späteren Zeitpunkt wieder.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
          break;
           
        case EUpdateResult.Error:
          MessageBox.Show(this, "Es ist ein Fehler aufgetreten." , "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
          this.Close();
          break;
      }
    }
  }
}