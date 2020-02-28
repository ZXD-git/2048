using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2048
{
    /// <summary>
    /// Control_Grid.xaml 的交互逻辑
    /// </summary>
    public partial class Control_Grid : UserControl
    {
        int Number = 0;
        string Color = "";
        public Control_Grid(int number,string color)
        {
            InitializeComponent();
            Number = number;
            Color = color;
            this.Loaded += Control_Grid_Loaded;
        }
        public void Control_Grid_Loaded(object e, RoutedEventArgs eventArgs) {
            txt_number.Text = Number.ToString();
            border.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(Color));
            Storyboard storyboard = new Storyboard();

            DoubleAnimation doubleAnimation_width = new DoubleAnimation();
            doubleAnimation_width.Duration = TimeSpan.FromSeconds(0.2);
            doubleAnimation_width.To = 60;
            Storyboard.SetTarget(doubleAnimation_width, border);
            Storyboard.SetTargetProperty(doubleAnimation_width, new PropertyPath(Border.WidthProperty));


            DoubleAnimation doubleAnimation_height = new DoubleAnimation();
            doubleAnimation_height.Duration = TimeSpan.FromSeconds(0.2);
            doubleAnimation_height.To = 60;
            Storyboard.SetTarget(doubleAnimation_height, border);
            Storyboard.SetTargetProperty(doubleAnimation_height, new PropertyPath(Border.HeightProperty));

            DoubleAnimation doubleAnimation_size = new DoubleAnimation();
            doubleAnimation_size.Duration = TimeSpan.FromSeconds(0.2);
            doubleAnimation_size.To = 14;
            Storyboard.SetTarget(doubleAnimation_size, txt_number );
            Storyboard.SetTargetProperty(doubleAnimation_size, new PropertyPath(TextBlock.FontSizeProperty));

            storyboard.Children.Add(doubleAnimation_width);
            storyboard.Children.Add(doubleAnimation_height);
            storyboard.Children.Add(doubleAnimation_size);
            storyboard.Begin();
        }

        public void j() {
            txt_number.Text = "10";
        }
    }
}
