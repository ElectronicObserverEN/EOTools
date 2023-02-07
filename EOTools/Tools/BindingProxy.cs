using System.Windows;

namespace EOTools.Tools;

// https://thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/
public class BindingProxy<T> : Freezable
{
    protected override Freezable CreateInstanceCore() => new BindingProxy<T>();

    public T DataContext
    {
        get => (T)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(nameof(DataContext), typeof(T), typeof(BindingProxy<T>), new UIPropertyMetadata(null));
}
