using Diary.Commands;
using Diary.Properties;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class SetDatabaseConnectionViewModel : ViewModelBase, IDataErrorInfo
    {
        public string ServerName { get; set; }
  
        public string DatabaseName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value;}
        } 
        private string _connectionString;
        public SetDatabaseConnectionViewModel()
        {         
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);  
        }

        private string BindConnectionString() { return $"Server={ServerName};Database={DatabaseName};User Id={UserId};Password={Password};"; }
        private void Confirm(object obj)
        {
            if (ValidCondition())
            {
                MessageBox.Show("nie wszystkie pola zostały podane");
                return;
            }
           _connectionString = BindConnectionString();
         
            CloseWindow(obj as Window);
            ConfirmRestart();
        }

        private void Close(object obj)
        {          
            CloseWindow(obj as Window); 
        }

        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand DisplayConnectionStringCommand { get; set; }
        public string Error
        {
            get
            {
                return null;
            }
        }
        public string this[string name]
        {
            get
            {
                string result = null;

                if (ValidCondition())
                {
                    result = "Pole nie może byc puste";
                }                 
                return result;
            }
        }


        private bool ValidCondition()
        {
           
            if (ServerName != null || DatabaseName != null || UserId != null || Password != null)
            {
                return false;
            }
            else
                return true;   
        }


        private void CloseWindow(Window window)
        {
            window.Close();           
        }
       
        private async void IsConnectionToDbPossible()
        {       
            using (var context = new ApplicationDbContext())
            {

                SplashScreen splashScreen = new SplashScreen("Pictures/Loading.png");
                splashScreen.Show(false, true);
                var IsDbConnectionSuccessful = await Task.Run( () => context.Database.Exists());
                if ( IsDbConnectionSuccessful == true)
                {
                    splashScreen.Close(TimeSpan.Zero);
                    MessageBox.Show("Połączenie bazy danych możliwe", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Settings.Default.Save();
                    RestartApplication();
                }      
                else
                {
                    splashScreen.Close(TimeSpan.Zero);
                    MessageBox.Show("Nieprawidłowe parametry bazy danych", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Settings.Default.Reset();
                }       
            } 
        }

        private void OverWriteUserProperties()
        {
            Settings.Default.ServerName = ServerName;
            Settings.Default.DatabaseName = DatabaseName;
            Settings.Default.User_Id = UserId;
            Settings.Default.Password = Password;

            IsConnectionToDbPossible();
        }
        
        private void RestartApplication()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private async Task ConfirmRestart()
        {
          
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync(
                "Potwierdzenie restartu aplikacji",
                $"Czy na pewno chcesz zaktualizować połączenie z bazą danych? \n Connection string: {_connectionString}",
                MessageDialogStyle.AffirmativeAndNegative);

            if (dialog != MessageDialogResult.Affirmative)
                return;
            OverWriteUserProperties();    
        }
    }
}
