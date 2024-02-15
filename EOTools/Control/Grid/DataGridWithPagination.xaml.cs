using System.Windows;
using System.Windows.Controls;

namespace EOTools.Control.Grid;

/// <summary>
/// Interaction logic for DataGridWithPagination.xaml
/// </summary>
public partial class DataGridWithPagination : UserControl
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        "ViewModel", typeof(PaginationViewModel), typeof(DataGridWithPagination), new PropertyMetadata(default(PaginationViewModel)));

    public PaginationViewModel ViewModel
    {
        get => (PaginationViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public DataGridWithPagination()
    {
        InitializeComponent();
    }
}