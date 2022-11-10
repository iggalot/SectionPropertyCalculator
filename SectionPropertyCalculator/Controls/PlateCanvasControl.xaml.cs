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
        //private ResizeAdorner _resizeAdorner = null;
        private CResizeAdorner _resizeAdorner = null;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // View model for the basis of this control
        public PlateViewModel ViewModel { get; set; }

        public bool IsActivated { get; set; } = false;

        // Is the control being hovered over
        public bool IsHovered { get; set; } = false;

        // Is the control clicked or activated
        public bool IsActiveResize { get; set; } = false;

        public int SCALE_FACTOR { get; set; } = 1;

        public double ScreenWidth { 
            get
            {
                return ViewModel.Model.Width * SCALE_FACTOR;
            } 
        }
        public double ScreenHeight {
            get
            {
                return ViewModel.Model.Height * SCALE_FACTOR;
            }
        }

        public Brush GetColor{ get => ViewModel.GetMaterialColor(ViewModel.Model.Material.MaterialType); }
        public double SetLeft { get => ViewModel.SetLeft; }
        public double SetTop { get => ViewModel.SetTop; }



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

        private void PlateControlClicked(object sender, RoutedEventArgs e)
        {
            if(IsActiveResize is false)
            {
                _resizeAdorner = new CResizeAdorner(RectControl, 300, 300);
                IsActiveResize = true;
            } else
            {
                this.RemoveResizeAdorner();
            }

            //// Raise the event to notify the main application that something has changed
            RaiseEvent(new RoutedEventArgs(PlateCanvasControl.OnControlModifiedEvent));
            
            
            //RaiseEvent(new RoutedEventArgs(PlateCanvasControl.OnControlClickedEvent));
        }

        /// <summary>
        /// Deactivates the resize adorner for this control
        /// </summary>
        public void RemoveResizeAdorner()
        {
            if (_resizeAdorner != null)
            {
                AdornerLayer aLayer = AdornerLayer.GetAdornerLayer(this);

                if (aLayer == null)
                {
                    // Clear the visual children
                    _resizeAdorner.visualChildren.Clear();

                    return;

                }

                Adorner[] toRemoveArray = aLayer.GetAdorners(PlateCanvasControlGrid);
                Adorner toRemove;

                if (toRemoveArray != null)
                {
                    for (int i = 0; i < toRemoveArray.Length; i++)
                    {
                        toRemove = toRemoveArray[i];
                        aLayer.Remove(toRemove);
                    }
                }

                // clear the adorner visual contents
                _resizeAdorner.visualChildren.Clear();
            }

            // delete the adorner in this control
            _resizeAdorner = null;

            IsActiveResize = false;

            Update();
            this.UpdateLayout();
        }

        /// <summary>
        /// Activates the resize adorner for this control
        /// </summary>
        public void AddResizeAdorner()
        {
            _resizeAdorner = new CResizeAdorner(RectControl, 300, 300);

            //_resizeAdorner = new ResizeAdorner(RectControl, ViewModel.Model.Id, ViewModel.Model.Centroid, ViewModel.Model.TopLeftPt, SCALE_FACTOR);
            //_resizeAdorner.OnAdornerModified += ResizeAdorner_UpdateRequired;

            //AdornerLayer.GetAdornerLayer(PlateCanvasControlGrid).Add(_resizeAdorner);

            IsActiveResize = true;

            Update();
            this.UpdateLayout();
        }

        private void ResizeAdorner_UpdateRequired(object sender, RoutedEventArgs e)
        {
            ResizeAdorner adorn = sender as ResizeAdorner;

            // Update the centroid with the new width and height
            ViewModel.Model.Width = adorn.ControlModelWidth_Current / SCALE_FACTOR;
            ViewModel.Model.Height = adorn.ControlModelHeight_Current / SCALE_FACTOR;

            ViewModel.Model.TopLeftPt = new Point(ViewModel.Model.Width + adorn.ControlModelLeftOffset, adorn.ControlModelRightOffset);

            ViewModel.Model.Centroid = adorn.ControlModelCentroid;

            //ViewModel.Model.Centroid = new Point(
            //    (ViewModel.SetLeft + 0.5 * (adorn.ControlWidth / SCALE_FACTOR)), 
            //    (ViewModel.SetTop + 0.5 * (adorn.ControlHeight / SCALE_FACTOR)));

            tbInfo.Text = ViewModel.Model.Centroid.X.ToString() + ":" + ViewModel.Model.Centroid.Y.ToString();
            Update();

            // Raise the event to notify the main application that something has changed
 //           RaiseEvent(new RoutedEventArgs(PlateCanvasControl.OnControlModifiedEvent));
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            _bIsLoaded = true;


        }
    }
}
