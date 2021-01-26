using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataConnectionTester
{
    public partial class MainForm : Form
    {
        private MainFormModel _model = new MainFormModel();
        private ErrorLog _logger = new ErrorLog();

        public MainForm()
        {
            InitializeComponent();
            BindControlsToModel();
        }

        private void BindControlsToModel()
        {

            txtConnection.DataBindings.Add(
                "Text", _model, "DefaultConnection", false, DataSourceUpdateMode.OnPropertyChanged );
            btnTest.DataBindings.Add(
                "Enabled", _model, "CanTryConnection", false, DataSourceUpdateMode.OnPropertyChanged );
            btnOpenLogFile.DataBindings.Add(
                "Enabled", _model, "CanOpenLogFile", false, DataSourceUpdateMode.OnPropertyChanged );

        }

        private void btnDone_Click( object sender, System.EventArgs e )
        {
            Close();
        }

        private void btnErrorTest_Click( object sender, System.EventArgs e )
        {
            var message =
                @"This is a null argument exception test, had this been a real exception something would be really messed up.";
            ErrorLog.Log( new ArgumentNullException( "MyArgument" ), message );
            _model.CanOpenLogFile = ErrorLog.LogFileExists;
        }

        private void btnTest_Click( object sender, EventArgs e )
        {
            var cur = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            var message = string.Empty;

            try
            {
                var formatString = "{0} to \"" + _model.DefaultConnection + "\"";

                if (_model.TestConnection())
                    message = string.Format( formatString, "Successfully connected " );
                else
                    message = string.Format( formatString, "Failed connection " );
            }
            finally
            {
                Cursor.Current = cur;
            }


            MessageBox.Show( message, @"Test Connection",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation );


        }

        private void btnOpenLogFile_Click( object sender, EventArgs e )
        {
            ErrorLog.OpenLogFile();
        }
    }
}
