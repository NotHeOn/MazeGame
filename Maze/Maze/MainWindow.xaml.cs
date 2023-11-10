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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Maze
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int time_remaining=300;
        int MHeight=9;
        int MWidth=12;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Game NewGame = new Game(time_remaining,MWidth,MHeight);         
            NewGame.Show();
            this.Close();           
        }

        private void Sets_Click(object sender, RoutedEventArgs e)
        {
            Setting set = new Setting(time_remaining,MWidth,MHeight);
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
