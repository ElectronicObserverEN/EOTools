using EOTools.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace EOTools.Translation
{
    /// <summary>
    /// Interaction logic for TagUpdaterForm.xaml
    /// </summary>
    public partial class TagUpdaterForm : Page
    {
        private TagListViewModel ViewModel => (DataContext as TagListViewModel);

        public TagUpdaterForm()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // --- Load file
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    (EoDataFolderPath.DataContext as TagListViewModel).ElectronicObserverDataFolderPath = dialog.SelectedPath;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button)
            {
                if (button.DataContext is LockData lockData)
                {
                    ViewModel.DeleteLockCommand.Execute(lockData);
                }
            } 
        }
    }
}