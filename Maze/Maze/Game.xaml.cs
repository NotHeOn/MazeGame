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
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Maze
{
    /// <summary>
    /// Game.xaml 的交互逻辑
    /// </summary>
    public partial class Game : Window
    {
        int MWidth = 12;  //迷宫地图宽度 默认为12
        int MHeight = 9; //迷宫地图高度 默认为9
        int[,] NumMap; //数字地图
        int[,] NumPath; //数字解
        int PlayerX = 0; //记录玩家位置
        int PlayerY = 0; //记录玩家位置
        int time_remaining=300; //时限 默认为300
        int Score=0; //得分
        bool CreateAgain = false; //判断是否为第二次创建地图
        bool MapCreate = false; //判读地图是否已经生成
        bool HasMoved = false; //判断玩家是否已经移动
        bool GameOver = false; //判断一局游戏是否已经结束
        private DispatcherTimer dispatcherTimer;

        //窗口初始化，接收主窗口的三个参数
        public Game(int time, int width, int height)
        {
            InitializeComponent();
            
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            time_remaining = time;
            MWidth = width;
            MHeight = height;

        }




        //方法--通过数字地图在Canvas面板绘制地图（包括绘制出玩家的起点位置）
        private void DrawingMap(int[,] NumMap)
        {
            for (int i = 0; i < MHeight; i++)
                for (int j = 0; j < MWidth; j++)
                {
                    Image image = new Image();
                    image.Margin = new Thickness(37 * j,37 * i, 0, 0);
                    string str = @"./ImageResource/Images/";
                    str += NumMap[i,j].ToString();str += ".jpg";
                    image.Source = new BitmapImage(new Uri(str, UriKind.Relative));
                    Map.Children.Add(image);
                }
            Image image1 = new Image();
            image1.Margin = new Thickness(0, 0, 0, 0);
            string str1 = @"./ImageResource/PlayerImages/p.png";
            image1.Source = new BitmapImage(new Uri(str1, UriKind.Relative));
            Map.Children.Add(image1);

        }

        //方法--创建一张新的地图（并标出时限）
        //等价于 开启一轮新的游戏
        private void Create_New_Map()
        {
            Map.Children.Clear();
            PlayerX = 0;
            PlayerY = 0;
            HasMoved = false;
            GameOver = false;

            MazeMap Map1 = new MazeMap(MWidth, MHeight);
            NumMap = Map1.GetNumMap();
            NumPath = Map1.GetNumPath();
            DrawingMap(NumMap);

            MapCreate = true;

            TimeBlock.Text = time_remaining.ToString();
        }

        //方法--根据数字解绘制出迷宫解
        private void ShowAnswer()
        {
            if (MapCreate)
            {
                for (int i = 0; i < MHeight; i++)
                    for (int j = 0; j < MWidth; j++)
                    {
                        if (NumPath[i, j] == 1 && (i != PlayerX || j != PlayerY))
                        {
                            Image image = new Image();
                            image.Margin = new Thickness(37 * j, 37 * i, 0, 0);
                            string str = @"./ImageResource/PlayerImages/p1.png";
                            image.Source = new BitmapImage(new Uri(str, UriKind.Relative));
                            Map.Children.Add(image);
                        }

                    }
            }
        }

        //方法--计时并判断玩家是否失败
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (int.Parse(TimeBlock.Text) - 1 >= 0)
            {
                TimeBlock.Text = (int.Parse(TimeBlock.Text) - 1).ToString();
            }
            else
            {
                GameOver = true;
                dispatcherTimer.Stop();
                string str = "时间到，游戏结束！\n是否开始新一轮游戏？";
                MessageBoxResult messageBoxResult;
                messageBoxResult = MessageBox.Show(str, "失败", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        Create_New_Map(); break;
                    case MessageBoxResult.No:
                        {
                            MessageBoxResult messageBoxResult1;
                            messageBoxResult1 = MessageBox.Show("是否显示答案", "答案", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                            switch (messageBoxResult1)
                            {
                                case MessageBoxResult.Yes: ShowAnswer(); break;
                            }
                            break;
                        }

                }

            }

        }

        //方法--计算分数
        private void ScoreCount()
        {
            if (time_remaining - int.Parse(TimeBlock.Text) != 0)
            {
                Score = (int)((MHeight * MWidth) * 100 + 1000 / (time_remaining - int.Parse(TimeBlock.Text)));
            }
            else
            {
                Score = (int)((MHeight * MWidth) * 100 + 1000);
            }
        }



        //按键--创建迷宫
        private void CreateMap_Click(object sender, RoutedEventArgs e)
        {
            if (!CreateAgain) { CreateMap.Content = "重 新 生 成";CreateAgain = true; }
            Create_New_Map();
            dispatcherTimer.Stop();
        }

        //按键-改写键盘事件，用于玩家走迷宫以及判断玩家是否成功
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (MapCreate)
            {
                if (!HasMoved && !GameOver) { HasMoved = true; dispatcherTimer.Start(); }
                int _PlayerX = PlayerX;
                int _PlayerY = PlayerY;
                int Direction = 0;
                Key key = e.Key;
                switch (key)
                {
                    case Key.Up:
                        _PlayerX--;
                        Direction = 8;
                        break;
                    case Key.Down:
                        _PlayerX++;
                        Direction = 2;
                        break;
                    case Key.Left:
                        _PlayerY--;
                        Direction = 1;
                        break;
                    case Key.Right:
                        _PlayerY++;
                        Direction = 4;
                        break;
                }
                if (_PlayerX < MHeight && _PlayerY < MWidth && _PlayerX >= 0 && _PlayerY >= 0 && (NumMap[PlayerX, PlayerY] & Direction) != 0)
                {

                    Image image1 = new Image();
                    image1.Margin = new Thickness(37 * PlayerY, 37 * PlayerX, 0, 0);
                    string str1= @".\ImageResource\Images\";
                    str1 += NumMap[PlayerX, PlayerY].ToString(); str1 += ".jpg";
                    image1.Source = new BitmapImage(new Uri(str1,UriKind.Relative));
                    
                    Map.Children.Add(image1);

                    Image image2 = new Image();
                    image2.Margin = new Thickness(37 * _PlayerY, 37 * _PlayerX, 0, 0);
                    string str2 = @".\ImageResource\PlayerImages\p.png";
                    image2.Source = new BitmapImage(new Uri(str2, UriKind.Relative));
                    
                    Map.Children.Add(image2);

                    PlayerX = _PlayerX;
                    PlayerY = _PlayerY;
                    
                }

                if (PlayerX == MHeight - 1 && PlayerY == MWidth - 1)
                {
                    
                    dispatcherTimer.Stop();
                    if (!GameOver)
                    {
                        GameOver = true;
                        string str = "恭喜您成功过关！您的得分为： ";
                        ScoreCount();
                        str += Score.ToString();
                        str += "\n是否开始新一轮游戏？";
                        MessageBoxResult messageBoxResult;
                        messageBoxResult = MessageBox.Show(str, "成功", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        switch (messageBoxResult)
                        {
                            case MessageBoxResult.Yes:
                                Create_New_Map(); break;
                        }
                    }
                }
            }
        }

        //按键--显示答案
        private void ShowAns_Click(object sender, RoutedEventArgs e)
        {
            GameOver = true;
            dispatcherTimer.Stop();
            ShowAnswer();
        }

        //按键--暂停游戏
        private void StopGame_Click(object sender, RoutedEventArgs e)
        {
            if(MapCreate)
            {
                dispatcherTimer.Stop();
                MessageBox.Show("游戏暂停中", "暂停", MessageBoxButton.OK, MessageBoxImage.Warning);
                dispatcherTimer.Start();
            }
        }

        //按键--游戏设置
        private void Sets_Click(object sender, RoutedEventArgs e)
        {
            if(MapCreate)
            {
                MessageBoxResult messageBoxResult;
                dispatcherTimer.Stop();
                messageBoxResult=MessageBox.Show("进行设置后迷宫将重新生成，是否继续？", "暂停", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        {
                            Setting set = new Setting(time_remaining, MWidth, MHeight);
                            set.ShowDialog();
                            bool result = (bool)set.DialogResult;
                            if (result)
                            {
                                time_remaining = set.time_remaining;
                                MHeight = set.MHeight;
                                MWidth = set.MWidth;
                            }
                            Create_New_Map();
                            break;
                        }
                    case MessageBoxResult.No:
                        dispatcherTimer.Start();
                        break;
                }
            }
            else
            {
                Setting set = new Setting(time_remaining, MWidth, MHeight);                
                set.ShowDialog();
                bool result = (bool)set.DialogResult;
                if (result)
                {
                    time_remaining = set.time_remaining;
                    MHeight = set.MHeight;
                    MWidth = set.MWidth;
                }
                
            }
        }


    }
}
