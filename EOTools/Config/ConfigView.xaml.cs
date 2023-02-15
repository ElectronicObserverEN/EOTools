using System.Windows.Controls;

namespace EOTools.Config
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class ConfigView : Page
    {
        public ConfigView()
        {
            InitializeComponent();

            DataContext = new ConfigViewModel();
        }
    }
}
