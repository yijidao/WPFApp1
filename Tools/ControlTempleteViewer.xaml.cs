using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace Tools
{
    /// <summary>
    /// ControlTempleteViewer.xaml 的交互逻辑
    /// </summary>
    public partial class ControlTempleteViewer : Window
    {
        public ControlTempleteViewer()
        {
            InitializeComponent();
        }

        private void ctlListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var type = (Type)ctlListView.SelectedItem;
            var constructorInfo = type.GetConstructor(System.Type.EmptyTypes);
            var control = (Control)constructorInfo.Invoke(null);
            control.Visibility = Visibility.Collapsed;
            grid.Children.Add(control);
            var controlTemplete = control.Template;

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings);
            XamlWriter.Save(controlTemplete, xmlWriter);

            txtBlock.Text = stringBuilder.ToString();

            grid.Children.Remove(control);
        }

        private void dllListView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var controlType = typeof(Control);
            var derivedTypes = new List<Type>();
            Assembly assembly = Assembly.GetAssembly(controlType);
            foreach (var type in assembly.GetTypes())
            {
                if(type.IsSubclassOf(controlType) && !type.IsAbstract && type.IsPublic)
                {
                    derivedTypes.Add(type);
                }
                ctlListView.ItemsSource = derivedTypes;
            }
        }
    }
}
