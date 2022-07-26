using Diary.Commands;
using Diary.Models;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class AddEditStudentViewModel : ViewModelBase
    {

        private Repository _repository = new Repository();
        public AddEditStudentViewModel(StudentWrapper student = null)
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);

            if (student == null)
            {
                Student = new StudentWrapper();
            }
            else
            {
                Student = student;
                IsUpdate = true;
            }

            InitGroups();
        }



        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }


        private StudentWrapper _student;

        public StudentWrapper Student
        {
            get { return _student; }
            set
            {
                _student = value;
                OnPropertyChanged();
            }


        }

        private bool _isUpdate;

        public bool IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
                OnPropertyChanged();
            }


        }


        private int _selectedGroupId;

        public int SelectedGroupId
        {
            get { return _selectedGroupId; }
            set { _selectedGroupId = value; }
        }

        private ObservableCollection<Group> _group;
        public ObservableCollection<Group> Groups
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged();
            }
        }

        private void InitGroups()
        {

            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "-- brak --" });

            Groups = new ObservableCollection<Group>(groups);


            SelectedGroupId = Student.Group.Id;
        }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);


        }

        private void Confirm(object obj)
        {

            if (!Student.IsValid)
            {
                return;
            }

            if (!IsUpdate)
                AddStudent();
            else
                UpdateStudent();
            
            CloseWindow(obj as Window);
        }

        private void AddStudent()
        {
            _repository.AddStudent(Student);
        }

        private void UpdateStudent()
        {
            _repository.UpdateStudent(Student);
           
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }



    }
}
//private void OverwriteConnectionStringUserSettings()
//{
//    Settings.Default.ServerName = ServerName;
//    Settings.Default.DatabaseName = DatabaseName;
//    Settings.Default.User_Id = UserId;
//    Settings.Default.Password = Password;
//}