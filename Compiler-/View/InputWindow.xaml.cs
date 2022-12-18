using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Compiler_.View
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public string Input { get; set; }

        public InputWindow()
        {
            InitializeComponent();

            tbInput.Background = new SolidColorBrush(Color.FromArgb(225, 32, 23, 62));
            tbInput.Foreground = new SolidColorBrush(Color.FromArgb(223, 236, 236, 236));

            inputWindow.Background = new SolidColorBrush(Color.FromArgb(199, 200, 191, 231));

            Input = string.Empty;
        }

        private void Submit(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Input = tbInput.Text;
            }
        }
    }
}
