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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MineButton[,] mineButtons = new MineButton[30,16];
        public static List<MineButton> Mines = new List<MineButton>();

        public static Tuple<int,int> Random()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            int row = rand.Next(16);
            int col = rand.Next(30);
            if (mineButtons[col, row].IsMine)
                return Random();
            else
                return new Tuple<int, int>(col, row);
        }
        
        public static void CreateRandomMines()
        {
            for (int num = 0; num < 99; num++)
            {
                var tuple = Random();
                int col = tuple.Item1;
                int row = tuple.Item2;
                if (!mineButtons[col, row].IsMine)
                {
                    mineButtons[col, row].IsMine = true;
                    Mines.Add(mineButtons[col, row]);
                }
            }
        }

        public void SetAroundButtons(int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                if (i < 0 || i >= 30)
                    continue;
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (j < 0 || j >= 16)
                        continue;
                    if (!mineButtons[i, j].IsMine)
                        mineButtons[i, j].Number++;
                }
            }
        }

        public void CreateMainZone()
        {
            for (int row = 0; row < 16; row++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(10, GridUnitType.Auto);
                this.MainZone.RowDefinitions.Add(rowDefinition);
                for (int col = 0; col < 30; col++)
                {
                    ColumnDefinition columnDefinition = new ColumnDefinition();
                    columnDefinition.Width = new GridLength(0, GridUnitType.Auto);
                    this.MainZone.ColumnDefinitions.Add(columnDefinition);
                    MineButton button = new MineButton();
                    button.Height = 25;
                    button.Width = 25;
                    button.row = row;
                    button.col = col;
                    button.IsMine = false;
                    button.Number = 0;
                    button.Background = Brushes.LightBlue;
                    button.Click += OnMineButtonClick;
                    Grid.SetColumn(button, col);
                    Grid.SetRow(button, row);

                    mineButtons[col, row] = button;

                    MainZone.Children.Add(button);
                }
            }

            CreateRandomMines();
            if (Mines.Count == 99)
            {
                foreach(var mine in Mines)
                {
                    SetAroundButtons(mine.col, mine.row);
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            CreateMainZone();
        }

        public void OnMineButtonClick(object sender, EventArgs e)
        {
            var button = sender as MineButton;
            if (button.IsMine)
            {
                button.Content = "M";
                button.Background = Brushes.Red;
            }

            else
            {
                button.Content = button.Number;
                button.Background = Brushes.LightGray;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            MainZone.Children.Clear();
            mineButtons = new MineButton[30, 16];
            Mines.Clear();
            CreateMainZone();
        }
    }
}
