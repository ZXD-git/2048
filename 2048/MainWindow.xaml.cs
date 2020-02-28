using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 存放控件的坐标
        /// </summary>
        List<Point_Border> point_Borders = new List<Point_Border>();
        readonly List<Point> Points = new List<Point>() {
          new Point(){ X=5,Y=5},
          new Point(){ X=5,Y=70},
          new Point(){ X=5,Y=135},
          new Point(){ X=5,Y=200},

          new Point(){ X=70,Y=5},
          new Point(){ X=70,Y=70},
          new Point(){ X=70,Y=135},
          new Point(){ X=70,Y=200},

           new Point(){ X=135,Y=5},
          new Point(){ X=135,Y=70},
          new Point(){ X=135,Y=135},
          new Point(){ X=135,Y=200},

           new Point(){ X=200,Y=5},
          new Point(){ X=200,Y=70},
          new Point(){ X=200,Y=135},
          new Point(){ X=200,Y=200}
      };//空闲位置的坐标
        static string[] BackColors = new string[] { "#f6f5ec", "#d9d6c3", "#d3d7d4", "#918597", "#FFEADEA4", "#FFBDD397", "#FFA1F582", "#FF90D5E0", "#FF5FA2BB", "#FF9E87DA", "#FFAF35DC", "#FFEE43C7", "#FFE63B21", "#FFF50F39" };
        public int NewName = 1;
        bool GameState = false;
        object o = new object();
        Score score = new Score() { Color= "#f6f5ec",Number=0 };
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            txt_number.DataContext = score;
        }

        /// <summary>
        /// 创建新的控件
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="colorIndex"></param>
        /// <param name="point"></param>
        private void Create(int Number, int colorIndex, Point point)
        {
            Control_Grid control = new Control_Grid(Number, BackColors[colorIndex]);
            Canvas.SetLeft(control, point.X);
            Canvas.SetTop(control, point.Y);
            control.Name = "board" + NewName;
            canvas.Children.Add(control);

            point_Borders.Add(new Point_Border()
            {
                ColorIndex = colorIndex,
                Name = "board" + NewName,
                Number = Number,
                Point = point
            });
            NewName++;

        }

        /// <summary>
        /// 生成数字
        /// </summary>
        private int Create_Number()
        {
            Random random = new Random();

            int u = random.Next(3);
            if (u > 1)
            {
                return 4;
            }
            else
            {
                return 2;
            }

        }

        /// <summary>
        /// 生成随机坐标
        /// </summary>
        /// <returns></returns>
        public Point Create_Point()
        {
            Random random = new Random();
            List<Point> point = new List<Point>() {
          new Point(){ X=5,Y=5},
          new Point(){ X=5,Y=70},
          new Point(){ X=5,Y=135},
          new Point(){ X=5,Y=200},

          new Point(){ X=70,Y=5},
          new Point(){ X=70,Y=70},
          new Point(){ X=70,Y=135},
          new Point(){ X=70,Y=200},

           new Point(){ X=135,Y=5},
          new Point(){ X=135,Y=70},
          new Point(){ X=135,Y=135},
          new Point(){ X=135,Y=200},

           new Point(){ X=200,Y=5},
          new Point(){ X=200,Y=70},
          new Point(){ X=200,Y=135},
          new Point(){ X=200,Y=200}
      };
            foreach (var item in point_Borders)
            {
                if (Points.Contains(item.Point))
                {
                    point.Remove(item.Point);
                }
            }

            if (point == null || point.Count == 0)
            {
                return new Point(0, 0);
            }
            int number = random.Next(point.Count);
            return point[number];
        }

        public void MainWindow_Loaded(object e, RoutedEventArgs sender)
        {
            

        }

       

        public bool IsGameOver() {
            if (point_Borders.Count < 16) {
                return true;
            }
            for (int i = 0; i < 4; i++) {
                for (int o = 0; o < 4; o++) {
                    if (point_Borders.Find(s => s.Point == new Point(65 * i + 5, 65 * o + 5)).Number == point_Borders.Find(s => s.Point == new Point(65 * i + 70>200?135: 65 * i + 70, 65 * o + 5)).Number|| point_Borders.Find(s => s.Point == new Point(65 * i + 5, 65 * o + 5)).Number== point_Borders.Find(s => s.Point == new Point(65 * i + 5, 65 * o + 70>200?135: 65 * o + 70)).Number) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 创建随机控件
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="color"></param>
        /// <param name="colorindex"></param>
        private bool CreateBoard()
        {
            int number = Create_Number();
            Control_Grid control = new Control_Grid(number, number == 2 ? BackColors[0] : BackColors[1]);
            string name = "board" + NewName;
            control.Name = name;
            Point point = Create_Point();
            if (point == new Point(0, 0))
            {
                return false;
            }
            Canvas.SetLeft(control, point.X);
            Canvas.SetTop(control, point.Y);
            canvas.Children.Add(control);
            point_Borders.Add(new Point_Border()
            {
                Name = name,
                Point = point,
                ColorIndex = number == 2 ? 0 : 1,
                Number = number
            });
            NewName++;
            return true;
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (!GameState) {
                return;
            }
            Key key = e.Key;
            if (key == Key.Left)
            {
                if (Run_Left())
                {
                    if (!CreateBoard())
                    {
                        MessageBox.Show("gameover");
                    };
                };
            }
            else if (key == Key.Right)
            {
                if (Run_Right())
                {
                    if (!CreateBoard())
                    {
                        MessageBox.Show("gameover");
                    };
                };
            }
            else if (key == Key.Up)
            {
                if (Run_Top())
                {
                    if (!CreateBoard())
                    {
                        MessageBox.Show("gameover");
                    };
                };
            }
            else if (key == Key.Down)
            {
                if (Run_Down())
                {
                    if (!CreateBoard())
                    {
                        MessageBox.Show("gameover");
                    };
                };
            }
            else {
                this.WindowState = WindowState.Minimized;
                return;
            }

            if (!IsGameOver()) {
                MessageBox.Show("GameOver!");
                point_Borders.Clear();
                canvas.Children.RemoveRange(16,16);
                score.Number = 0;
                CreateBoard();

                Dispatcher.BeginInvoke((Action)delegate {
                    string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;//获取或设置包含该应用程序的目录的名称。
                    path += "File";
                    if (!Directory.Exists(path)) {
                        Directory.CreateDirectory(path);
                    }
                    path += "//MyRank.txt";
                    StreamWriter stream = new StreamWriter(path, false);
                    stream.WriteLine(score.Number + ";");
                    stream.Close();
                });
            }
        }



        #region 移动
        private bool Run_Left()
        {
            int number = 0;
            bool run = false;
            lock (o) {
                for (int i = 0; i < 4; i++)
                {
                    List<Point_Border> borders = point_Borders.FindAll(s => s.Point.Y == 65 * i + 5).OrderBy(s => s.Point.X).ToList();
                    if (borders != null && borders.Count > 0)
                    {
                        bool p = false;//表示上一步合并没有
                        int u = 0;//表示合并次数
                        for (int o = 0; o < borders.Count; o++)
                        {
                            if (borders[o].Point.X == 65 * o + 5 && u == 0)
                            {
                                if (o == 0 || (borders[o].Number != borders[o - 1].Number && o > 0))
                                {
                                    continue;
                                }
                            }
                            run = true;
                            Storyboard storyboard = new Storyboard();
                            DoubleAnimation doubleAnimation = new DoubleAnimation();
                            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);

                            if (o > 0 && borders[o].Number == borders[o - 1].Number && !p)
                            {
                                u++;
                                p = true;
                                number += 2 * borders[o].Number;
                                doubleAnimation.To = 65 * (o - u) + 5;
                                #region  添加动画
                                foreach (var item in canvas.Children)
                                {
                                    if (item is Control_Grid control_Grid)
                                    {
                                        if (control_Grid.Name == borders[o].Name)
                                        {
                                            Storyboard.SetTarget(doubleAnimation, control_Grid);
                                            break;
                                        }
                                    }
                                }
                                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Canvas.LeftProperty));
                                storyboard.Children.Add(doubleAnimation);
                                storyboard.Begin();
                                #endregion

                                #region 移出控件
                                List<Control_Grid> controls = new List<Control_Grid>();
                                foreach (var item in canvas.Children)
                                {
                                    if (item is Control_Grid grid)
                                    {
                                        if (grid.Name == borders[o].Name || grid.Name == borders[o - 1].Name)
                                        {
                                            controls.Add(grid);
                                        }
                                    }
                                }
                                foreach (var item in controls)
                                {
                                    canvas.Children.Remove(item);
                                }

                                point_Borders.RemoveAll(s => s.Name == borders[o].Name || s.Name == borders[o - 1].Name);
                                #endregion

                                Create(2 * borders[o].Number, borders[o].ColorIndex + 1, new Point(65 * (o - u) + 5, 65 * i + 5));
                                continue;
                            }
                            doubleAnimation.To = 65 * (o - u) + 5;
                            p = false;
                            #region 添加动画
                            foreach (var item in canvas.Children)
                            {
                                if (item is Control_Grid control_Grid)
                                {
                                    if (control_Grid.Name == borders[o].Name)
                                    {
                                        Storyboard.SetTarget(doubleAnimation, control_Grid);
                                        break;
                                    }
                                }
                            }
                            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Canvas.LeftProperty));
                            storyboard.Children.Add(doubleAnimation);
                            storyboard.Begin();
                            #endregion

                            #region 修改控件的坐标
                            point_Borders.FindAll(s => s.Name == borders[o].Name).ForEach(s => s.Point = new Point()
                            {
                                X = 65 * (o - u) + 5,
                                Y = 65 * i + 5
                            });
                            #endregion

                        }
                    }
                }
            }
           
            score.Number += number;
            return run;
        }

        private bool Run_Right()
        {
            int number = 0;
            bool run = false;

            lock (o)
            {
                for (int i = 0; i < 4; i++)
                {
                    List<Point_Border> borders = point_Borders.FindAll(s => s.Point.Y == 65 * i + 5).OrderByDescending(s => s.Point.X).ToList();
                    if (borders != null && borders.Count > 0)
                    {
                        bool p = false;//表示上一步合并没有
                        int u = 0;//表示合并次数
                        for (int o = 0; o < borders.Count; o++)
                        {
                            if (borders[o].Point.X == 200 - 65 * o && u == 0)
                            {
                                if (o == 0 || (borders[o].Number != borders[o - 1].Number && o > 0))
                                {
                                    continue;
                                }
                            }
                            run = true;
                            var t = canvas.FindName(borders[o].Name);
                            Storyboard storyboard = new Storyboard();
                            DoubleAnimation doubleAnimation = new DoubleAnimation();
                            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);

                            if (o > 0 && borders[o].Number == borders[o - 1].Number && !p)
                            {
                                number += 2 * borders[o].Number;
                                u++;
                                p = true;
                                doubleAnimation.To = 200 - 65 * (o - u);
                                #region 添加动画
                                foreach (var item in canvas.Children)
                                {
                                    if (item is Control_Grid control_Grid)
                                    {
                                        if (control_Grid.Name == borders[o].Name)
                                        {
                                            Storyboard.SetTarget(doubleAnimation, control_Grid);
                                            break;
                                        }
                                    }
                                }
                                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Canvas.LeftProperty));
                                storyboard.Children.Add(doubleAnimation);
                                storyboard.Begin();
                                #endregion

                                #region 移出控件
                                List<Control_Grid> controls = new List<Control_Grid>();
                                foreach (var item in canvas.Children)
                                {
                                    if (item is Control_Grid grid)
                                    {
                                        if (grid.Name == borders[o].Name || grid.Name == borders[o - 1].Name)
                                        {
                                            controls.Add(grid);
                                        }
                                    }
                                }
                                foreach (var item in controls)
                                {
                                    canvas.Children.Remove(item);
                                }
                                point_Borders.RemoveAll(s => s.Name == borders[o].Name || s.Name == borders[o - 1].Name);
                                #endregion

                                Create(2 * borders[o].Number, borders[o].ColorIndex + 1, new Point(200 - 65 * (o - u), 65 * i + 5));

                                continue;
                            }
                            p = false;
                            doubleAnimation.To = 200 - 65 * (o - u);
                            #region 添加动画
                            foreach (var item in canvas.Children)
                            {
                                if (item is Control_Grid control_Grid)
                                {
                                    if (control_Grid.Name == borders[o].Name)
                                    {
                                        Storyboard.SetTarget(doubleAnimation, control_Grid);
                                        break;
                                    }
                                }
                            }
                            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Canvas.LeftProperty));
                            storyboard.Children.Add(doubleAnimation);
                            storyboard.Begin();
                            #endregion

                            #region 修改控件坐标
                            point_Borders.FindAll(s => s.Name == borders[o].Name).ForEach(s => s.Point = new Point()
                            {
                                X = 200 - 65 * (o - u),
                                Y = 65 * i + 5
                            });
                            #endregion

                        }
                    }
                }
            }
            
            score.Number += number;
            return run;
        }

        private bool Run_Top()
        {
            int number = 0;
            bool run = false;
            lock (o) {
                for (int i = 0; i < 4; i++)
                {
                    List<Point_Border> borders = point_Borders.FindAll(s => s.Point.X == 65 * i + 5).OrderBy(s => s.Point.Y).ToList();
                    if (borders != null && borders.Count > 0)
                    {

                        bool p = false;//表示上一步合并没有
                        int u = 0;//表示合并次数
                        for (int o = 0; o < borders.Count; o++)
                        {
                            if (borders[o].Point.Y == 65 * o + 5 && u == 0)
                            {
                                if (o == 0 || (borders[o].Number != borders[o - 1].Number && o > 0))
                                {
                                    continue;
                                }
                            }
                            run = true;
                            Storyboard storyboard = new Storyboard();
                            DoubleAnimation doubleAnimation = new DoubleAnimation();
                            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);

                            if (o > 0 && borders[o].Number == borders[o - 1].Number && !p)
                            {
                                number += 2 * borders[o].Number;
                                u++;
                                p = true;
                                doubleAnimation.To = 65 * (o - u) + 5;

                                #region 添加动画
                                foreach (var item in canvas.Children)
                                {
                                    if (item is Control_Grid control_Grid)
                                    {
                                        if (control_Grid.Name == borders[o].Name)
                                        {
                                            Storyboard.SetTarget(doubleAnimation, control_Grid);
                                            break;
                                        }
                                    }
                                }
                                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Canvas.TopProperty));
                                storyboard.Children.Add(doubleAnimation);
                                storyboard.Begin();
                                #endregion

                                #region 移出控件
                                List<Control_Grid> controls = new List<Control_Grid>();
                                foreach (var item in canvas.Children)
                                {
                                    if (item is Control_Grid grid)
                                    {
                                        if (grid.Name == borders[o].Name || grid.Name == borders[o - 1].Name)
                                        {
                                            controls.Add(grid);
                                        }
                                    }
                                }
                                foreach (var item in controls)
                                {
                                    canvas.Children.Remove(item);
                                }
                                point_Borders.RemoveAll(s => s.Name == borders[o].Name || s.Name == borders[o - 1].Name);
                                #endregion

                                Create(borders[o].Number * 2, borders[o].ColorIndex + 1, new Point(65 * i + 5, 65 * (o - u) + 5));

                                continue;
                            }
                            doubleAnimation.To = 65 * (o - u) + 5;
                            p = false;

                            #region 添加动画
                            foreach (var item in canvas.Children)
                            {
                                if (item is Control_Grid control_Grid)
                                {
                                    if (control_Grid.Name == borders[o].Name)
                                    {
                                        Storyboard.SetTarget(doubleAnimation, control_Grid);
                                        break;
                                    }
                                }
                            }
                            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Canvas.TopProperty));
                            storyboard.Children.Add(doubleAnimation);
                            storyboard.Begin();
                            #endregion

                            #region 修改控件坐标
                            point_Borders.FindAll(s => s.Name == borders[o].Name).ForEach(s => s.Point = new Point()
                            {
                                X = 65 * i + 5,
                                Y = 65 * (o - u) + 5
                            });
                            #endregion

                        }
                    }
                }
            }
            
            score.Number += number;
            return run;
        }

        private bool Run_Down()
        {
            int number = 0;
            bool run = false;
            lock (o) {
                for (int i = 0; i < 4; i++)
                {
                    List<Point_Border> borders = point_Borders.FindAll(s => s.Point.X == 65 * i + 5).OrderByDescending(s => s.Point.Y).ToList();
                    if (borders != null && borders.Count > 0)
                    {

                        bool p = false;//表示上一步合并没有
                        int u = 0;//表示合并次数
                        for (int o = 0; o < borders.Count; o++)
                        {
                            if (borders[o].Point.Y == 200 - 65 * o && u == 0)
                            {
                                if (o == 0 || (borders[o].Number != borders[o - 1].Number && o > 0))
                                {
                                    continue;
                                }
                            }
                            run = true;
                            Storyboard storyboard = new Storyboard();
                            DoubleAnimation doubleAnimation = new DoubleAnimation();
                            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2);
                            if (o > 0 && borders[o].Number == borders[o - 1].Number && !p)
                            {
                                number += 2 * borders[o].Number;
                                u++;
                                p = true;
                                doubleAnimation.To = 200 - 65 * (o - u);

                                #region 添加动画
                                foreach (var item in canvas.Children)
                                {
                                    if (item is Control_Grid control_Grid)
                                    {
                                        if (control_Grid.Name == borders[o].Name)
                                        {
                                            Storyboard.SetTarget(doubleAnimation, control_Grid);
                                            break;
                                        }
                                    }
                                }
                                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Canvas.TopProperty));
                                storyboard.Children.Add(doubleAnimation);
                                storyboard.Begin();
                                #endregion

                                #region 移出控件
                                List<Control_Grid> controls = new List<Control_Grid>();
                                foreach (var item in canvas.Children)
                                {
                                    if (item is Control_Grid grid)
                                    {
                                        if (grid.Name == borders[o].Name || grid.Name == borders[o - 1].Name)
                                        {
                                            controls.Add(grid);
                                        }
                                    }
                                }
                                foreach (var item in controls)
                                {
                                    canvas.Children.Remove(item);
                                }
                                point_Borders.RemoveAll(s => s.Name == borders[o].Name || s.Name == borders[o - 1].Name);
                                #endregion

                                Create(2 * borders[o].Number, borders[o].ColorIndex + 1, new Point(65 * i + 5, 200 - 65 * (o - u)));

                                continue;
                            }
                            doubleAnimation.To = 200 - 65 * (o - u);
                            p = false;

                            #region 添加动画
                            foreach (var item in canvas.Children)
                            {
                                if (item is Control_Grid control_Grid)
                                {
                                    if (control_Grid.Name == borders[o].Name)
                                    {
                                        Storyboard.SetTarget(doubleAnimation, control_Grid);
                                        break;
                                    }
                                }
                            }
                            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Canvas.TopProperty));
                            storyboard.Children.Add(doubleAnimation);
                            storyboard.Begin();
                            #endregion


                            #region 修改控件坐标
                            point_Borders.FindAll(s => s.Name == borders[o].Name).ForEach(s => s.Point = new Point()
                            {
                                X = 65 * i + 5,
                                Y = 200 - 65 * (o - u)
                            });
                            #endregion

                        }
                    }
                }
            }
           
            score.Number += number;
            return run;
        }
        #endregion

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 窗体拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 新的游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            panel.Visibility = Visibility.Collapsed;
            canvas.Visibility = Visibility.Visible;
            GameState = true;
            CreateBoard();
        }

        /// <summary>
        /// 窗体关闭时  保存当前结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            if (point_Borders == null || point_Borders.Count == 0)
            {
                return;
            }
            string note = Json.ObjectToJson(point_Borders);
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;//获取或设置包含该应用程序的目录的名称。
            path += "File";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += "\\Note.txt";
            StreamWriter writer = new StreamWriter(path, false);
            writer.Write(note);
            writer.Close();
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;//获取或设置包含该应用程序的目录的名称。
                path += "File";
                if (!Directory.Exists(path))
                {
                    panel.Visibility = Visibility.Collapsed;
                    canvas.Visibility = Visibility.Visible;
                    GameState = true;
                    CreateBoard();
                    return;
                }
                path += "\\Note.txt";
                StreamReader stream = new StreamReader(path);
                string note = stream.ReadToEnd();
                stream.Close();

                point_Borders = Json.JsonToObject<List<Point_Border>>(note, new List<Point_Border>());
                foreach (var item in point_Borders)
                {
                    Control_Grid control = new Control_Grid(item.Number, BackColors[item.ColorIndex]);
                    Canvas.SetLeft(control, item.Point.X);
                    Canvas.SetTop(control, item.Point.Y);
                    control.Name = item.Name;
                    canvas.Children.Add(control);
                }
                GameState = true;
                panel.Visibility = Visibility.Collapsed;
                canvas.Visibility = Visibility.Visible;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
           
        }

        /// <summary>
        /// 查看战绩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;//获取或设置包含该应用程序的目录的名称。
            path += "File";
            if (!Directory.Exists(path))
            {
                
                return;
            }
        }

        private void Button_Click_3()
        {

        }
    }


    public class Point_Border
    {
        public Point Point { get; set; }
        public string Name { get; set; }
        public int ColorIndex { get; set; }
        public int Number { get; set; }
    }

    public class Score : INotifyPropertyChanged
    {
        private string color;
        private int number;
        public string Color { get { return color; } set { color = value; NotifyPropertyChange("Color"); } }
        public int Number
        {
            get { return number; }
            set
            {
                number = value; NotifyPropertyChange("Number");
                //if (Number > 1000)
                //{
                //    Color = "#f6f5ec";
                //}
                //else if (Number > 2500)
                //{
                //    Color = "#d9d6c3";
                //}
                //else if (Number > 6000)
                //{
                //    Color = "#d3d7d4";
                //}
                //else {
                //    Color = "#918597";
                //}
            }
        }

        /// <summary>
        /// 属性自动更新
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
