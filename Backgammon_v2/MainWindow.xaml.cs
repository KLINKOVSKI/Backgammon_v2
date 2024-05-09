using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Backgammon_v2
{
    public delegate void SpikeClicked(int row, int column, int cube1, int cube2);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int cube1;
        private int cube2;
        private Board LogicBoard;
        private UiSpikeElement[,] UiBoard;
        public static event SpikeClicked OnSpikeClicked;


        public MainWindow()
        {
            InitializeComponent();
            mariaDB newConnection = new mariaDB();
            newConnection.Connect();
            //newConnection.createDB();
            //newConnection.CreateUser("Ivan","1234");
            //newConnection.Close();
            //newConnection.testConnection();
            //newConnection.createDB();
            //newConnection.createUser();
            //newConnection.listTable();
            LogicBoard = new Board();
            UiBoard = new UiSpikeElement[2, 12];
            Board.RollCubeEvent += BoardOnRollCubeEvent;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Spike spike = LogicBoard[i, j];
                    UiBoard[i, j] = new UiSpikeElement(i, j);
                    UiBoard[i, j].MouseDown += OnMouseDown;
                    // UiBoard[i, j].MouseEnter += OnMouseEnter;
                    // UiBoard[i, j].MouseLeave += OnMouseLeave;
                    SetInGrid(UiBoard[i, j], i, j);
                    UiBoard[i, j].Update(spike);

                }
            }

        }

        private (int cub1, int cub2) BoardOnRollCubeEvent()
        {
            Cubes_Clicked(null, null);
            return (cube1, cube2);
        }

        private void SetInGrid(UiSpikeElement element, int row, int column)
        {

            if (column > 5)
            {
                column++;
            }
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            BoardGrid.Children.Add(element);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            UiSpikeElement uiSpikeElement = (UiSpikeElement)sender;
            OnSpikeClicked?.Invoke(uiSpikeElement.Row, uiSpikeElement.Column, cube1, cube2);
            Update();
        }

        private void Update()
        {
            Turn.Content = LogicBoard.BlackTurn ? "Black turn" : "White turn";
            if (LogicBoard.numTurns == -1)
            {
                LeftTurns.Content = "Left turn: 0";
            }
            else
            {
                LeftTurns.Content = $"Left turn: {LogicBoard.numTurns}";
            }
            EatenWhites.Content = $"Eaten whites: {LogicBoard.eatenW}";
            EatenBlacks.Content = $"Eaten blacks: {LogicBoard.eatenB}";

            WhitesOut.Content = $"Whites Out: {LogicBoard.whitesOut}";
            BlacksOut.Content = $"Blacks Out: {LogicBoard.blacksOut}";

            if (LogicBoard.blacksOut == 15)
            {
                MessageBox.Show("Black wins");

                Close();
            }
            if (LogicBoard.whitesOut == 15)
            {
                MessageBox.Show("White wins");
                Close();
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    UiBoard[i, j].Update(LogicBoard[i, j]);
                }
            }
        }

        private void Cubes_Clicked(object sender, RoutedEventArgs e)
        {
            Random cube = new Random();
            cube1 = cube.Next(1, 7);
            cube2 = cube.Next(1, 7);
            TextCube1.Content = cube1;
            TextCube2.Content = cube2;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("System checking who starts");

            do
            {
                Cubes_Clicked(null, null);
            } while (cube1 == cube2);

            MessageBox.Show(cube1 > cube2 ? "Black starts" : "White starts");
            LogicBoard.BlackTurn = (cube1 > cube2);
            Turn.Content = LogicBoard.BlackTurn ? "Black turn" : "White turn";
        }
    }
}