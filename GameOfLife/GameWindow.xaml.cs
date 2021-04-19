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

            for (int i=0;i < 50; i++)
            {
                ColumnDefinition kolom = new ColumnDefinition();                
                RowDefinition rij = new RowDefinition();                              
                LifeGrid.ColumnDefinitions.Add(kolom);
                LifeGrid.RowDefinitions.Add(rij);
            }

            Cell cell;
           // Rectangle background;
            for (int row = 0; row < LifeGrid.RowDefinitions.Count; row++)
            {
                for (int column = 0; column < LifeGrid.ColumnDefinitions.Count; column++)
                {
                    //background= new Rectangle();

                    //Grid.SetRow(background,row);
                    //Grid.SetColumn(background,column);
                    //LifeGrid.Children.Add(background);

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
            /* Compleet inefficient 
           foreach (Cell neighbour in LifeGrid.Children)
                {
                    int x = neighbour.Row;
                    int y = neighbour.Column;
                    if ((x == a - 1 && (y == b - 1 || y == b || y == b + 1))
                        || (x == a && (y == b - 1 || y == b + 1))
                        || (x == a + 1 && (y == b - 1 || y == b || y == b + 1)))
                    {
                        if (neighbour.Alive == true)
                            count++;
                    }
                }
             * Compleet inefficient 2
             * foreach (Cell neighbour in LifeGrid.Children.Cast<UIElement>().Where(neighbour => Grid.GetRow(neighbour) == y
                            && Grid.GetColumn(neighbour) == x))
                        {


                            if (cell.Column != x || cell.Row != y) 
         */

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
     
        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            Next();
            Update();
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

                int wacht = (int)DelaySlider.Value;
                Next();
                Update();
                await Task.Delay(wacht);
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            foreach (Cell cell in LifeGrid.Children)
                cell.NextAlive = false;
            Update();
        }

        private void StartingPos_Click(object sender, RoutedEventArgs e)
        {
            foreach (Cell reset in LifeGrid.Children)
            {
                reset.NextAlive = false;
            }
            foreach (Cell savedCell in startpos)
            {
                foreach (Cell cell in LifeGrid.Children)
                {
                    if (cell.Row == savedCell.Row && cell.Column == savedCell.Column)
                        cell.NextAlive = true;
                   
                }
            }
            Update();
        }

        private void SavePos_Click(object sender, RoutedEventArgs e)
        {
            startpos.Clear();
            foreach (Cell cell in LifeGrid.Children)
            {
                if (cell.Alive == true)
                startpos.Add(cell);
            }
        }
    }
}
