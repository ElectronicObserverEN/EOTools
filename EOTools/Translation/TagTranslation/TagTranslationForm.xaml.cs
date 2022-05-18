using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace EOTools.Translation
{
    /// <summary>
    /// Interaction logic for TagTranslationForm.xaml
    /// </summary>
    public partial class TagTranslationForm : Page
    {
        public TagTranslationForm()
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
    }
}
