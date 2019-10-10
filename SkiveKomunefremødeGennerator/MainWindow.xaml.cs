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
using SkiveKomunefremødeGennerator.Helpers;
using SkiveKomunefremødeGennerator.Model;

namespace SkiveKomunefremødeGennerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> fileNames;
        public MainWindow()
        {
            InitializeComponent();
            fileNames = new List<string> { "Skive", "Struer" };
            lbElever.ItemsSource = DB.GetStudents();
            lbSkema.ItemsSource = fileNames;
        }

        private void Knap_Click(object sender, RoutedEventArgs e)
        {

            if (lbElever.SelectedIndex != -1 && dpFrom.Text != "" && dpTo.Text != "" && lbSkema.SelectedIndex != -1)
            {
                if (lbSkema.SelectedItem as string == "Skive")
                {
                    Student s = lbElever.SelectedItem as Student;
                    DateTime? from = dpFrom.SelectedDate;
                    DateTime? to = dpTo.SelectedDate;
                    List<DagsRegistrering> liste = DB.getDagsReg(s, from, to);

                    string fileName = SkiveKomuneGenerator.Createworddocument(liste, lbSkema.SelectedItem as string);
                    MessageBoxResult result = MessageBox.Show("færdig vil du åbne dokumentet", "Info", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        SkiveKomuneGenerator.OpenExcel(fileName);
                    }
                }
                else if (lbSkema.SelectedItem as string == "Struer")
                {
                    Student s = lbElever.SelectedItem as Student;
                    DateTime? from = dpFrom.SelectedDate;
                    DateTime? to = dpTo.SelectedDate;
                    List<DagsRegistrering> liste = DB.getDagsReg(s, from, to);

                    string fileName = StruerKomuneGenerator.Createworddocument(liste, lbSkema.SelectedItem as string);
                    MessageBoxResult result = MessageBox.Show("færdig vil du åbne dokumentet", "Info", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        SkiveKomuneGenerator.OpenExcel(fileName);
                    }

                }
            }
        }
    }
}
