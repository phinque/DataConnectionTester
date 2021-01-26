using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DataConnectionTester
{
    public class ErrorLog
    {
        private  static string _logFile;
        public static bool LogFileExists => File.Exists( _logFile );

        public static  void Log( Exception exception )
        {
            Log( GetDefaultLogFileName(), exception );
        }
        public static void Log( Exception exception, string additionText )
        {
            Log( GetDefaultLogFileName(), exception, additionText );
        }

        public static void Log( string fileName, Exception exception )
        {
            using (var fs = File.AppendText( fileName ))
                fs.Write( GetExceptionString( exception ) );
            _logFile = fileName;
        }

        public static void Log( string fileName, Exception exception, string additionalText )
        {
            using (var fs = File.AppendText( fileName ))
            {
                fs.Write( Environment.NewLine );
                fs.Write( Environment.NewLine );
                fs.Write( additionalText );
                fs.Write( Environment.NewLine );
                fs.Write( GetExceptionString( exception ) );
            }
            _logFile = fileName;
        }

        private static string GetDefaultLogFileName()
        {
            // for now, put error logs in MyDocuments
            return 
                Path.Combine(
                    Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ),
                    $@"ErrorLog_{DateTime.Now.Year,2:00}{DateTime.Now.Month,2:00}{DateTime.Now.Day,2:00}.log" );
        }

        private static string GetExceptionString( Exception exception )
        {

            StringBuilder str = new StringBuilder();
            try
            {
                var ex = exception;
                str.Append( "Message: " );
                str.Append( ex.Message );
                str.Append( " (" );
                str.Append( ex.GetType().FullName );
                str.Append( ")" );
                str.Append( Environment.NewLine );

                while ((ex = ex.InnerException) != null)
                {
                    str.Append( "Inner message: " );
                    str.Append( ex.Message );
                    str.Append( " (" );
                    str.Append( ex.GetType().FullName );
                    str.Append( ")" );
                    str.Append( Environment.NewLine );
                }

                str.Append( "Type: " );
                str.Append( exception.GetType().FullName );
                str.Append( Environment.NewLine );

                str.Append( "Source: " );
                str.Append( exception.Source );
                str.Append( Environment.NewLine );

                str.Append( "Method: " );
                str.Append( exception.TargetSite );
                str.Append( Environment.NewLine );

                str.Append( "Stack Trace:" );
                str.Append( Environment.NewLine );
                str.Append( exception.StackTrace );
                str.Append( Environment.NewLine );

            }
            catch
            {
                //at least try to pass the message
                str.Append( "An exception occurred trying to log the text: " + exception.Message );
            }
            return str.ToString();
        }

        public static void OpenLogFile()
        {
            var fi = new FileInfo( _logFile );
            if (fi.Exists)
            {
                System.Diagnostics.Process.Start( _logFile );
            }
            else
            {
                MessageBox.Show( $@"{_logFile} not found.", @"File not found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }

    }


}
