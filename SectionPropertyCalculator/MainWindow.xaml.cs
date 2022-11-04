using SectionPropertyCalculator.Controls;
using SectionPropertyCalculator.Models;
using SectionPropertyCalculator.ViewModels;
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

namespace SectionPropertyCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CompositeSectionViewModel CompositeSectionVM { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            OnUserCreate();

            OnUserUpdate();
        }

        /// <summary>
        /// Runs once per application at startup
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnUserCreate()
        {
            CompositeShapeModel shape_model = new CompositeShapeModel();

            PlateModel model1 = new PlateModel(1, 6, 4, new Point(5, 2));
            shape_model.AddPlate(model1);

            PlateModel model2 = new PlateModel(2, 2, 10, new Point(1, 5));
            shape_model.AddPlate(model2);

            // Recreate the view model
            CompositeSectionVM = new CompositeSectionViewModel(shape_model);
        }

        /// <summary>
        /// Runs everytime something is changed or updatedd
        /// </summary>
        private void OnUserUpdate()
        {
            // Clear our layouts and prepare for redrawing
            cCanvasControls.Children.Clear();
            spPlateDataControls.Children.Clear();
            spResultsControls.Children.Clear();

            // recreate the controls
            foreach (PlateModel item in CompositeSectionVM.CompShapeModel.Plates)
            {
                PlateViewModel view_model = new PlateViewModel(item);
                PlateDataControl pdc_uc = new PlateDataControl(view_model);
                spPlateDataControls.Children.Add(pdc_uc);

                // Add to the canvas and hook up click events
                PlateCanvasControl pcc_uc = new PlateCanvasControl(view_model);
                AddPlateCanvasControlEvents(pcc_uc);
                cCanvasControls.Children.Add(pcc_uc);
                Canvas.SetLeft(pcc_uc, pcc_uc.SetLeft);
                Canvas.SetTop(pcc_uc, pcc_uc.SetTop);

                // Add to the composite section view model
                CompositeSectionVM.Add(view_model);
            }
        }


        protected void AddPlateCanvasControlEvents(UserControl uc)
        {
            // set up the events for the plate controls on the drawing canvas
            ((PlateCanvasControl)uc).OnControlModified += UpdateCanvasEvent;

        }

        private void UpdateCanvasEvent(object sender, RoutedEventArgs e)
        {
            // retrieve the view model in the sender
            PlateCanvasControl pcc = sender as PlateCanvasControl;

            // delete the control

            // recreate the control and adorner

            Console.WriteLine("\nNew dims: " + pcc.ViewModel.ScreenWidth + "  :  " + pcc.ViewModel.Model.Height);

            // update the view model

            // update the controls

            //Button control = sender as Button;
            //control.Background = Brushes.Red;
            //MessageBox.Show("Control was clicked");
        }
    }
}
