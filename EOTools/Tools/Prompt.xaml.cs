using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EOTools.Tools
{
  /// <summary>
  /// Interaction logic for Prompt.xaml
  /// </summary>
  public partial class Prompt : Window
  {
    public string ResultText 
    { 
      get
      {
        return ResponseTextBox.Text;
      }
    }

    public Prompt(string _name, string _promptLabel)
    {
      InitializeComponent();

      Title = _name;

      PromptLabel.Text = _promptLabel;
    }

    private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      DialogResult = true;
    }
  }
}
