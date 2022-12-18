using Compiler.CodeAnalysis.Compilation;
using Compiler.CodeAnalysis.Statements;
using Compiler.CodeAnalysis.Syntax.TreeStructure.Models;
using Compiler.CodeAnalysis.Text;
using Compiler_.Models.CodeAnalysis.Symbols;
using Compiler_.ViewModels;
using Compler.CodeAnalysis;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Compiler_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            backgroundWindow.Background = new SolidColorBrush(Color.FromArgb(199, 200, 191, 231));

            btnOpen.Background = new SolidColorBrush(Color.FromArgb(225, 32, 23, 62));
            btnOpen.Foreground = new SolidColorBrush(Color.FromArgb(223, 236, 236, 236));

            btnRun.Background = new SolidColorBrush(Color.FromArgb(225, 32, 23, 62));
            btnRun.Foreground = new SolidColorBrush(Color.FromArgb(223, 236, 236, 236));

            btnSave.Background = new SolidColorBrush(Color.FromArgb(225, 32, 23, 62));
            btnSave.Foreground = new SolidColorBrush(Color.FromArgb(223, 236, 236, 236));

            tbInput.Background = new SolidColorBrush(Color.FromArgb(225, 32, 23, 62));
            tbInput.Foreground = new SolidColorBrush(Color.FromArgb(223, 236, 236, 236));

            tbOutput.Background = new SolidColorBrush(Color.FromArgb(225, 32, 23, 62));
            tbOutput.Foreground = new SolidColorBrush(Color.FromArgb(223, 236, 236, 236));
        }

        private void Run(object sender, RoutedEventArgs e)
        {
            try
            {
                var input = tbInput.Text;
                var representation = new Representation(input);
                tbOutput.Text = representation.TextOutput;
            }
            catch (Exception ex)
            {
                tbOutput.Text = ex.Message;
            }
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            var fileName = dialog.FileName;
            if (Path.GetExtension(fileName).Equals(".sm"))
            {
                var text = File.ReadAllText(fileName);
                tbInput.Text = text;
            }
            else
            {
                MessageBox.Show("Unknown type");
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.ShowDialog();
            var fileName = $"{dialog.FileName}.sm";
            File.WriteAllText(fileName, tbInput.Text);
        }
    }
}
