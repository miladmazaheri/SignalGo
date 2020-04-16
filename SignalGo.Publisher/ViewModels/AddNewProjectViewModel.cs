﻿using System;
using System.IO;
using System.Linq;
using MvvmGo.Commands;
using MvvmGo.ViewModels;
using System.Windows.Forms;
using SignalGo.Publisher.Models;

namespace SignalGo.Publisher.ViewModels
{
    public class AddNewProjectViewModel : BaseViewModel
    {
        public AddNewProjectViewModel()
        {
            CancelCommand = new Command(Cancel);
            SaveCommand = new Command(Save);
            BrowseProjectPathCommand = new Command(BrowseProjectPath);
            BrowseAssembliesPathCommand = new Command(BrowseAssembliesPath);

        }

        public Command CancelCommand { get; set; }
        public Command SaveCommand { get; set; }
        public Command BrowseProjectPathCommand { get; set; }
        public Command BrowseAssembliesPathCommand { get; set; }

        string _Name;
        string _ProjectAssembliesPath;
        string _ProjectPath;
        Guid _ProjectKey = Guid.NewGuid();

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string ProjectAssembliesPath
        {
            get
            {
                return _ProjectAssembliesPath;
            }
            set
            {
                _ProjectAssembliesPath = value;
                OnPropertyChanged(nameof(ProjectAssembliesPath));
            }
        }
        public string ProjectPath
        {
            get
            {
                return _ProjectPath;
            }
            set
            {
                _ProjectPath = value;
                OnPropertyChanged(nameof(ProjectPath));
            }
        }

        public Guid ProjectKey
        {
            get => _ProjectKey;
            set
            {
                _ProjectKey = value;
                OnPropertyChanged(nameof(ProjectKey));
            }
        }

        private void Cancel()
        {
            ProjectManagerWindowViewModel.MainFrame.GoBack();
        }

        private void BrowseProjectPath()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = folderBrowserDialog.SelectedPath;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                ProjectPath = folderBrowserDialog.SelectedPath;
            }
        }

        private void BrowseAssembliesPath()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = folderBrowserDialog.SelectedPath;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                ProjectAssembliesPath = folderBrowserDialog.SelectedPath;
            }
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(this.Name))
                System.Windows.MessageBox.Show("Plase set name of project");
            else if (!Directory.Exists(ProjectPath))
                System.Windows.MessageBox.Show("files not exist on disk");
            else if (!Directory.Exists(ProjectAssembliesPath))
                System.Windows.MessageBox.Show("files not exist on disk");
            else if (SettingInfo.Current.ProjectInfo.Any(x => x.Name == Name))
                System.Windows.MessageBox.Show("Project name exist on list, please set a different name");
            else
            {
                SettingInfo.Current.ProjectInfo.Add(new ProjectInfo()
                {
                    ProjectKey = this.ProjectKey,
                    Name = Name,
                    ProjectPath = ProjectPath,
                    ProjectAssembliesPath = ProjectAssembliesPath
                });
                SettingInfo.SaveSettingInfo();
                ProjectManagerWindowViewModel.MainFrame.GoBack();
            }
        }
    }
}
