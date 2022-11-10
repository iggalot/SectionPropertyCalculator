using SectionPropertyCalculator.Controls;
using SectionPropertyCalculator.Controls.Adorners;
using SectionPropertyCalculator.Models;
using SectionPropertyCalculator.ViewModels;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SectionPropertyCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _bWindowisLoaded = false;
        CompositeSectionViewModel CompositeSectionVM { get; set; }
        CompositeShapeModel CompShapeModel { get; set; } = new CompositeShapeModel();

        public MainWindow()
        {
            InitializeComponent();

            OnUserCreate();

            DataContext = CompositeSectionVM;

            OnUserUpdate();
        }

        /// <summary>
        /// Runs once per application at startup
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnUserCreate()
        {
            // Create some plates
            PlateModel model1 = new PlateModel(1, 200, 200, new Point(0,0));
            CompShapeModel.AddPlate(model1);

            PlateModel model2 = new PlateModel(2, 50, 200, new Point(300, 300));
            CompShapeModel.AddPlate(model2);

            //PlateModel model3 = new PlateModel(3, 200, 40, new Point(300, 200));
            //CompShapeModel.AddPlate(model3);

            // Finnaly create the view model
            CreateCompositeViewModel();
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

            // Recreate  the composite view model
            CreateCompositeViewModel();

            // recreate the controls
            foreach (PlateViewModel item in CompositeSectionVM.lstPlateViewModel)
            {
                PlateDataControl pdc_uc = new PlateDataControl(item);
                spPlateDataControls.Children.Add(pdc_uc);

                // Add to the canvas and hook up click events
                PlateCanvasControl pcc_uc = new PlateCanvasControl(item);
                AddPlateCanvasControlEvents(pcc_uc);
                cCanvasControls.Children.Add(pcc_uc);
                Canvas.SetLeft(pcc_uc, pcc_uc.SetLeft);
                Canvas.SetTop(pcc_uc, pcc_uc.SetTop);
            }
        }

        /// <summary>
        /// Creates the composite view model for our  constructed model and sets the data context
        /// </summary>
        protected void CreateCompositeViewModel()
        {
            CompositeSectionVM = new CompositeSectionViewModel(CompShapeModel, cCanvasControls);
            DataContext = CompositeSectionVM;
        }

        protected void AddPlateCanvasControlEvents(UserControl uc)
        {
            // set up the events for the plate controls on the drawing canvas
            ((PlateCanvasControl)uc).OnControlModified += UpdatePlateModelInfo;
            ((PlateCanvasControl)uc).OnControlClicked += DeactivateCanvasControls;
        }

        private void DeactivateCanvasControls(object sender, RoutedEventArgs e)
        {
            // Retrieve the control of the sender
            PlateCanvasControl pcc = sender as PlateCanvasControl;

            foreach(PlateCanvasControl item in cCanvasControls.Children)
            {
                // skip the control if it is the sender, as it is already turned on or off in the pcc class.
                if ((pcc.ViewModel.Model.Id == item.ViewModel.Model.Id))
                {
                    continue;
                } 
                // Otherwise turn of all active controls
                else
                {
                    ((PlateCanvasControl)item).RemoveResizeAdorner();
                }
            }
        }


        private void UpdatePlateModelInfo(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("\n--------------------");
            foreach (FrameworkElement fe in cCanvasControls.Children)
            {
                double top = (double)fe.GetValue(Canvas.TopProperty);
                double left = (double)fe.GetValue(Canvas.LeftProperty);

                fe.SetCurrentValue(Canvas.TopProperty, Canvas.GetTop(fe) - 50);
                fe.SetCurrentValue(Canvas.LeftProperty, Canvas.GetLeft(fe) - 50);

                Console.WriteLine("--coords: " + top.ToString() + " : " + left.ToString());
            }

            // retrieve the control in the sender
            PlateCanvasControl pcc = sender as PlateCanvasControl;

            // find the plate id in the composite model list
            for (int i = 0; i < CompShapeModel.Plates.Count; i++)
            {
                int id = CompShapeModel.Plates[i].Id;

                // if the ids match, update the dimensions in the composite model list
                if(id == pcc.ViewModel.Model.Id)
                {
                    CompShapeModel.Plates[i].Height = pcc.ScreenHeight / pcc.SCALE_FACTOR;
                    CompShapeModel.Plates[i].Width = pcc.ScreenWidth / pcc.SCALE_FACTOR;

                    break;
                }
            }

            pcc.Update();

           // CreateCompositeViewModel();
            CompositeSectionVM.Update();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new CResizeAdorner(button1, 300, 300);
            new CResizeAdorner(datagrid1, 300, 300);
            new CResizeAdorner(textbox1, 300, 300);

        }
    }
}
