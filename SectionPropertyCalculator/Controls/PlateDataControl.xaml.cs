using SectionPropertyCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SectionPropertyCalculator.Controls
{
    /// <summary>
    /// Interaction logic for PlateDataControl.xaml
    /// </summary>
    public partial class PlateDataControl : UserControl, INotifyPropertyChanged
    {
        public PlateViewModel ViewModel { get; set; }

        public PlateDataControl(PlateViewModel view_model)
        {
            ViewModel = view_model;

            InitializeComponent();
        }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
