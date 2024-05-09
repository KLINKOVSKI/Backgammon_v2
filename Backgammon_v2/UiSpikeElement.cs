using Backgammon_v2;
using System;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Backgammon_v2
{
    public class UiSpikeElement : Label
    {
        public int Row { get; }
        public int Column { get; }

        private const int Size = 75;

        public UiSpikeElement(int row, int column)
        {
            Row = row;
            Column = column;
            Background = new ImageBrush
            {
                ImageSource = GetBackgroundImage(row, column)
            };
        }

        public void Update(int soldierCount, bool black)
        {
            string path = $"Images/{(black ? "Black" : "White")}Player.png";
            StackPanel stackPanel = new StackPanel();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.EndInit();
            for (int i = 0; i < soldierCount; i++)
            {
                stackPanel.Children.Add(new Image { Source = image });
            }
            Content = stackPanel;
            VerticalContentAlignment = Row == 0 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
        }

        public void Update(Spike spike)
        {
            StackPanel stackPanel = new StackPanel();

            // Clear existing children
            stackPanel.Children.Clear();

            // Add red or white dots based on the player's color
            for (int i = 0; i < spike.SoldiersCount; i++)
            {
                // Create a red or white dot (Ellipse) to represent a soldier
                Ellipse soldierDot = new Ellipse
                {
                    Width = 50, // Increase size
                    Height = 50, // Increase size
                    Fill = spike.Black ? Brushes.Red : Brushes.White,
                    Margin = new Thickness(5) // Add margin for spacing between dots
                };

                // Center the dot horizontally within its container
                soldierDot.HorizontalAlignment = HorizontalAlignment.Center;

                stackPanel.Children.Add(soldierDot);
            }

            // Set the content of the UiSpikeElement to the stackPanel containing soldier dots
            Content = stackPanel;

            // Set vertical alignment based on row
            VerticalContentAlignment = Row == 0 ? VerticalAlignment.Top : VerticalAlignment.Bottom;

            // Set background image
            BitmapImage image = new BitmapImage();
            if (spike.Marked)
            {
                image = GetMarkedImage(Row, Column);
            }
            else if (spike.PreviewMode)
            {
                image = GetPreviewImage(Row, Column);
            }
            else if (spike.OutMode)
            {
                image = GetOutModeImage(Row, Column);
            }
            else
            {
                image = GetBackgroundImage(Row, Column);
            }
            Background = new ImageBrush
            {
                ImageSource = image
            };
        }


        //try to bring to foregroung soldiers here
        //frfrfebgzbgvnjzngjnrjgnkjnvkzsnvgkjsznrgjknrjkvgnsjkvgnjknvgjsnvgksngjkdrnjkdne
        private static BitmapImage GetSoldierImage(Spike spike)
        {
            string path = $"Images/{(spike.Black ? "Black" : "White")}Player.png";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.EndInit();
            return image;
        }

        private BitmapImage GetPreviewImage(int row, int column)
        {
            string path = $"Images/{((column + row) % 2 == 0 ? "Brown" : "Black")}SpikeMarked.png";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.Rotation = Row == 0 ? Rotation.Rotate180 : Rotation.Rotate0;
            image.EndInit();
            return image;
        }

        private BitmapImage GetMarkedImage(int row, int column)
        {
            string path = $"Images/Marked{((column + row) % 2 == 0 ? "Brown" : "Black")}.png";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.Rotation = Row == 0 ? Rotation.Rotate180 : Rotation.Rotate0;
            image.EndInit();
            return image;
        }

        private BitmapImage GetBackgroundImage(int row, int column)
        {
            string path = $"Images/{((column + row) % 2 == 0 ? "Brown" : "Black")}Spike.png";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.Rotation = Row == 0 ? Rotation.Rotate180 : Rotation.Rotate0;
            image.EndInit();
            return image;
        }

        public override string ToString()
        {
            return $"Row: {Row}, Column: {Column}";
        }
        private BitmapImage GetOutModeImage(int row, int column)
        {
            string path = $"Images/Out{((column + row) % 2 == 0 ? "Brown" : "Black")}Spike.png";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.Rotation = Row == 0 ? Rotation.Rotate180 : Rotation.Rotate0;
            image.EndInit();
            return image;
        }
    }
}