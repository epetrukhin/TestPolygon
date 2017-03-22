using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp
{
    internal sealed partial class MainWindow
    {
        private readonly Model _model = new Model();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _model;
        }
    }

	public sealed class FooConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var model = (Model)value;
			return model.Flag;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
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