using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;
using DataConnectionTester.Annotations;

namespace DataConnectionTester
{
    public class MainFormModel : INotifyPropertyChanged
    {
        private bool _canOpenLogFile;
        private bool _canTryConnection;
        private string _defaultConnection;

        public MainFormModel()
        {
            DefaultConnection = Properties.Settings.Default.DefaultConnection;
        }

        
        public bool CanOpenLogFile
        {
            get => _canOpenLogFile;
            set
            {
                _canOpenLogFile = value;
                OnPropertyChanged();
            }
        }

        public bool CanTryConnection
        {
            get => _canTryConnection;
            set
            {
                _canTryConnection = value; 
                OnPropertyChanged();
            }
        }

        public string DefaultConnection
        {
            get => _defaultConnection;
            set
            {
                _defaultConnection = value;
                CanTryConnection = _defaultConnection.Length > 0;
                OnPropertyChanged(nameof(CanTryConnection));
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (var sqlConnection = new SqlConnection( DefaultConnection ))
                {
                    var timeout = sqlConnection.ConnectionTimeout;
                    sqlConnection.Open();
                    sqlConnection.Close();
                }

            }
            catch (Exception exception)
            {
                ErrorLog.Log( exception );
                CanTryConnection = false;
                return false;
            }
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

    }

}
