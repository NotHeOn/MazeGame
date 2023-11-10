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
using System.Windows.Shapes;

namespace Maze
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        public int time_remaining=300;
        public int MWidth =12;
        public int MHeight =9;
        public Setting(int time, int width, int height)
        {
            InitializeComponent();
            time_remaining = time;
            MWidth = width;
            MHeight = height;
            TimeBox.Text = time_remaining.ToString();
            MWidthBox.Text = MWidth.ToString();
            MHeightBox.Text = MHeight.ToString();

        }
        private void Yse_Btn_Click(object sender, RoutedEventArgs e)
        {
            bool InputRight0 = false;
            bool InputRight1 = false;
            bool InputRight2 = false;
            try
            {
                if (1 <= int.Parse(TimeBox.Text) && int.Parse(TimeBox.Text) <= 1000)
                {
                    time_remaining = int.Parse(TimeBox.Text); InputRight0 = true;
                }
                else
                {
                    MessageBox.Show("请正确输入时限！", "注意", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("请正确输入时限！", "注意", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                if (2 <= int.Parse(MWidthBox.Text) && int.Parse(MWidthBox.Text) <= 18)
                {
                    MWidth = int.Parse(MWidthBox.Text);InputRight1 = true;
                }
                else
                {
                    MessageBox.Show("请正确输入宽度！", "注意", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("请正确输入宽度！", "注意", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                if (2 <= int.Parse(MHeightBox.Text) && int.Parse(MHeightBox.Text) <= 12)
                {
                    MHeight = int.Parse(MHeightBox.Text);InputRight2 = true;
                }
                else
                {
                    MessageBox.Show("请正确输入高度！", "注意", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("请正确输入高度！", "注意", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (InputRight1 && InputRight2 && InputRight0)
            {
                this.DialogResult = true;
            }
        }
        private void No_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
