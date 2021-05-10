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
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Collections;

namespace GameOfLife
{

    public partial class GameWindow : Window
    {
        ArrayList startpos = new ArrayList();

        public GameWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {          
           LifeGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
           LifeGrid.VerticalAlignment = VerticalAlignment.Stretch;

            CreateGrid(16, 9);
        }

        public void CreateGrid(int height, int width)
        {
            for (int i = 0; i < width; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                LifeGrid.ColumnDefinitions.Add(col);
            }
            for (int i = 0; i < height; i++)
            {
                RowDefinition row = new RowDefinition();
                LifeGrid.RowDefinitions.Add(row);
            }

            Cell cell;
            // Rectangle background;
            for (int row = 0; row < LifeGrid.RowDefinitions.Count; row++)
            {
                for (int column = 0; column < LifeGrid.ColumnDefinitions.Count; column++)
                {
                    cell = new Cell(false, row, column);
                    cell.Opacity = 0;
                    cell.Width = LifeGrid.Width / LifeGrid.ColumnDefinitions.Count;
                    cell.Height = LifeGrid.Height / LifeGrid.RowDefinitions.Count;
                    cell.SetValue(Grid.ColumnProperty, column);
                    cell.SetValue(Grid.RowProperty, row);
                    LifeGrid.Children.Add(cell);
                }
            }
        }

        private void MultiAlive_MouseMove(object sender, MouseButtonEventArgs e)
        {

        }

        public void Next()
        {
            foreach (Cell cell in LifeGrid.Children)
            {              
                int count = 0;
                for (int y = cell.Row - 1; y <= cell.Row + 1; y++)
                {
                    if (y >= 0 && y <= LifeGrid.RowDefinitions.Count - 1)                      
                    {
                        for (int x = cell.Column - 1; x <= cell.Column + 1; x++)
                        {
                            if (x >= 0 && x <= LifeGrid.ColumnDefinitions.Count-1)
                            {
                                Cell neighbour = LifeGrid.Children[y * LifeGrid.ColumnDefinitions.Count + x ] as Cell;
                                if (neighbour.Alive == true && (cell.Column != x || cell.Row != y))
                                    count++;
                            }
                        }
                    }
                }              
                cell.NextState(count);
            }
        }
            
        private void Update()
        {
            foreach (Cell cell in LifeGrid.Children)
            {
                cell.Alive = cell.NextAlive;
                if (cell.Alive == true)
                    cell.Opacity = 1;

                   
                else
                    cell.Opacity = 0;
            }
        }

        private async void Autostep_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton auto = (ToggleButton)sender;
            await Autorun(auto);           
        }

        private async Task Autorun(ToggleButton runbutton)
        {
            while (runbutton.IsChecked == true)
            {

                int delay = 200;
                Next();
                Update();
                await Task.Delay(delay);
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            foreach (Cell cell in LifeGrid.Children)
                cell.NextAlive = false;
            Update();
        }

        private void btnBuildGrid_Click(object sender, RoutedEventArgs e)
        {
            string height = tbxHeight.Text;
            string width = tbxWidth.Text;

            if (Int32.TryParse(height, out int resultHeight))
            {
                if (Int32.TryParse(width, out int resultWidth))
                {
                    LifeGrid.RowDefinitions.Clear();
                    LifeGrid.ColumnDefinitions.Clear();
                    LifeGrid.Children.Clear();
                    CreateGrid(resultHeight, resultWidth);
                    tbxWidth.Background = System.Windows.Media.Brushes.White;
                    tbxHeight.Background = System.Windows.Media.Brushes.White;
                    return;
                }
            }
            tbxHeight.Background = System.Windows.Media.Brushes.Red;
            tbxWidth.Background = System.Windows.Media.Brushes.Red;
        }

        private void btnFillGridRandom_Click(object sender, RoutedEventArgs e)
        {
            foreach (Cell cell in Lifegrid.Children) {
                Random random = new Random();
                int number = random.Next(0, 4);

                cell.NextAlive = false;

                if (number == 4) {
                    cell.NextAlive = true;
                }

                cell.ChangeState();
            }
        }
    }
}
