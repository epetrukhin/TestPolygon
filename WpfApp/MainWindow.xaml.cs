using System;
using System.Globalization;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp
{
    internal sealed partial class MainWindow
    {
        // private readonly Model _model = new Model();

        public MainWindow()
        {
            InitializeComponent();

            // DataContext = _model;

            var source1 = GetTextChangesFor(txtSource1);
            var source2 = GetTextChangesFor(txtSource2);

            source1
                .CombineLatest(source2, (x, y) => $"{x} - {y}")
                .Subscribe(Log);
        }

        private static IObservable<string> GetTextChangesFor(TextBox textBox) =>
            Observable
                .FromEvent<TextChangedEventHandler, TextChangedEventArgs>(
                    h => (TextChangedEventHandler)((_, e) => h(e)),
                    h => textBox.TextChanged += h,
                    h => textBox.TextChanged -= h)
                .Select(_ => textBox.Text);


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            txtLog.Text = string.Empty;
        }

        private void Log(string text)
        {
            txtLog.Text += $"{DateTime.Now:mm:ss} {text}{Environment.NewLine}";
        }
    }

    public sealed class FooConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var model = (Model)value;
            return model.Flag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }

    internal sealed class Model : ModelBase
    {
        private bool flag;

        public bool Flag
        {
            get { return flag; }
            set { SetProperty(ref flag, value); }
        }
    }
}