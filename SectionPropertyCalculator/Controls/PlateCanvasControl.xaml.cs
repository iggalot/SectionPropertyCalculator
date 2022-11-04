using SectionPropertyCalculator.Controls.Adorners;
using SectionPropertyCalculator.Models;
using SectionPropertyCalculator.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SectionPropertyCalculator.Controls
{

    /// <summary>
    /// Interaction logic for PlateCanvasControl.xaml
    /// </summary>
    public partial class PlateCanvasControl : UserControl, INotifyPropertyChanged
    {
        private bool _bIsLoaded = false;
        private ResizeAdorner _resizeAdorner = null;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // View model for the basis of this control
        public PlateViewModel ViewModel { get; set; }

        // Is the control being hovered over
        public bool IsHovered { get; set; } = false;

        // Is the control clicked or activated
        public bool IsActiveResize { get; set; } = false;

        public int SCALE_FACTOR { get; set; } = 25;

        public double ScreenWidth { 
            get
            {
                return ViewModel.Model.Width * SCALE_FACTOR;
                //if(IsActiveResize == true)
                //{
                //    return 100;
                //} else
                //{
                //    return 20;
                //}

  //              Update();
            } 
        }
        public double ScreenHeight {
            get
            {
                return ViewModel.Model.Height * SCALE_FACTOR;

                //if (IsActiveResize == true)
                //{
                //    return 100;
                //}
                //else
                //{
                //    return 20;
                //}

                //               Update();
            }
        }

        public Brush GetColor{ get => ViewModel.GetMaterialColor(ViewModel.Model.Material.MaterialType); }
        public double SetLeft { get => ViewModel.SetLeft * SCALE_FACTOR; }
        public double SetTop { get => ViewModel.SetLeft * SCALE_FACTOR; }



        // create events to notify the parent that something has changed
        public static readonly RoutedEvent OnControlClickedEvent = EventManager.RegisterRoutedEvent("PlateControlClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlateCanvasControl));
        public static readonly RoutedEvent OnControlModifiedEvent = EventManager.RegisterRoutedEvent("UpdatePlateControl", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlateCanvasControl));


        // expose our event
        public event RoutedEventHandler OnControlClicked
        {
            add { AddHandler(OnControlClickedEvent, value); }
            remove
            {
                RemoveHandler(OnControlClickedEvent, value);
            }
        }

        // expose our event
        public event RoutedEventHandler OnControlModified
        {
            add { AddHandler(OnControlModifiedEvent, value); }
            remove
            {
                RemoveHandler(OnControlModifiedEvent, value);
            }
        }

        public PlateCanvasControl()
        {

        }

        public PlateCanvasControl(PlateViewModel view_model)
        {
            InitializeComponent();

            ViewModel = view_model;

            DataContext = this;

            // Add the control click event
            PlateCanvasControlGrid.MouseLeftButtonUp += PlateControlClicked;
        }

        public void Update()
        {
            OnPropertyChanged("ScreenWidth");
            OnPropertyChanged("ScreenHeight");
            OnPropertyChanged("GetColor");
            OnPropertyChanged("IsActiveResize");
            OnPropertyChanged("SetLeft");
            OnPropertyChanged("SetTop");

        }

        //private void ContentControl_MouseEnter(object sender, RoutedEventArgs e)
        //{
        //    IsHovered = true;
        //}

        //private void ContentControl_MouseLeave(object sender, RoutedEventArgs e)
        //{
        //    IsHovered = false;
        //}

        //private void ContentControl_Clicked(object sender, RoutedEventArgs e)
        //{
        //    IsActiveResize = true;
        //    RaiseEvent(new RoutedEventArgs(OnControlClickedEvent, this));
        //}

        private void PlateControlClicked(object sender, RoutedEventArgs e)
        {
//            PlateCanvasControl control = (PlateCanvasControl)sender;

            if(_bIsLoaded is true)
            {
                if (IsActiveResize is true)
                {
                    if (_resizeAdorner != null)
                    {
                        Adorner[] toRemoveArray = myAdornerLayer.AdornerLayer.GetAdorners(PlateCanvasControlGrid);
                        Adorner toRemove;

                        if(toRemoveArray != null)
                        {
                            for(int i = 0; i < toRemoveArray.Length; i++)
                            {
                                toRemove = toRemoveArray[i];
                                myAdornerLayer.AdornerLayer.Remove(toRemove);
                            }
                        }

                        // clear the adorner visual contents
                        _resizeAdorner.AdornerVisuals.Clear();
                    }

                    // delete the adorner in this control
                    _resizeAdorner = null;

                    IsActiveResize = false;
                }
                else
                {
                    _resizeAdorner = new ResizeAdorner(RectControl, ViewModel.Model.Id, SCALE_FACTOR);
                    _resizeAdorner.OnAdornerModified += ReiszeAdorner_UpdateRequired;

                    AdornerLayer.GetAdornerLayer(PlateCanvasControlGrid).Add(_resizeAdorner);

                    IsActiveResize = true;
                }

//               MessageBox.Show("Active: " + IsActiveResize.ToString());


                Update();
                this.UpdateLayout();
            }

            // Raise the event to notify the main application that something has changed
            RaiseEvent(new RoutedEventArgs(PlateCanvasControl.OnControlModifiedEvent));


        }

        private void ReiszeAdorner_UpdateRequired(object sender, RoutedEventArgs e)
        {
            ResizeAdorner adorn = sender as ResizeAdorner;
            ViewModel.Model.Width = adorn.Width / SCALE_FACTOR;
            ViewModel.Model.Height = adorn.Height / SCALE_FACTOR;

            Update();

            // Raise the event to notify the main application that something has changed
            RaiseEvent(new RoutedEventArgs(PlateCanvasControl.OnControlModifiedEvent));


        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            //PlateCanvasControl control = (PlateCanvasControl)sender;

            _bIsLoaded = true;

           // MessageBox.Show(IsActiveResize.ToString());
            //this.UpdateLayout();
        }
    }
}
