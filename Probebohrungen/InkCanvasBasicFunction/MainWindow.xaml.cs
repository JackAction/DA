
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InkCanvasBasicFunction
{
    ///<summary>
    /// Interaction logic for MainWindow.xaml
    ///</summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            // Get a reference to the radiobutton
            var radiobutton = sender as RadioButton;

            // Get the radiobutton pressed
            string radioBPressed = radiobutton.Content.ToString();

            // Change settings based on button
            if (radioBPressed == "Draw")
            {
                this.DrawingCanvas.EditingMode = InkCanvasEditingMode.Ink;
            }
            else if (radioBPressed == "Erase")
            {
                this.DrawingCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            }
            else if (radioBPressed == "Select")
            {
                this.DrawingCanvas.EditingMode = InkCanvasEditingMode.Select;
            }
        }

        private void DrawPanel_KeyUp(object sender, KeyEventArgs e)
        {
            if ((int)e.Key >= 35 && (int)e.Key
<= 68)
            {
                switch ((int)e.Key)
                {
                    case 35:
                        strokeAttr.Width = 2;
                        strokeAttr.Height = 2;
                        break;
                    case 36:
                        strokeAttr.Width = 4;
                        strokeAttr.Height = 4;
                        break;
                    case 37:
                        strokeAttr.Width = 6;
                        strokeAttr.Height = 6;
                        break;
                    case 38:
                        strokeAttr.Width = 8;
                        strokeAttr.Height = 8;
                        break;
                    case 39:
                        strokeAttr.Width = 10;
                        strokeAttr.Height = 10;
                        break;
                    case 40:
                        strokeAttr.Width = 12;
                        strokeAttr.Height = 12;
                        break;
                    case 41:
                        strokeAttr.Width = 14;
                        strokeAttr.Height = 14;
                        break;
                    case 42:
                        strokeAttr.Width = 16;
                        strokeAttr.Height = 16;
                        break;
                    case 43:
                        strokeAttr.Width = 18;
                        strokeAttr.Height = 18;
                        break;
                    case 45:
                        strokeAttr.Color = (Color)ColorConverter.ConvertFromString("Blue");
                        break;
                    case 50:
                        strokeAttr.Color = (Color)ColorConverter.ConvertFromString("Green");
                        break;
                    case 68:
                        strokeAttr.Color = (Color)ColorConverter.ConvertFromString("Yellow");
                        break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream("MyPicture.bin", FileMode.Create))
            {
                this.DrawingCanvas.Strokes.Save(fs);
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream("MyPicture.bin", FileMode.Open, FileAccess.Read))
            {
                StrokeCollection sc = new StrokeCollection(fs);
                this.DrawingCanvas.Strokes = sc;
            }
        }
    }
}