using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SectionPropertyCalculator.Controls.Adorners
{
    /// <summary>
    /// A class for creating adorner icons for resizing a framework element.
    /// </summary>
    public class ResizeAdorner : Adorner
    {
        public VisualCollection AdornerVisuals;
        Thumb thumb1, thumb2, platecanvas_thumb1, platecanvas_thumb2;
        Rectangle Rec;
        TextBlock txtBlock_VertDim;
        TextBlock txtBlock_HorizDim;
        TextBlock txtBlock_PlateNum;


        /// <summary>
        /// Details for the adorner
        /// </summary>
        public double SCREEN_SCALE_FACTOR;
        public double ControlWidth;
        public double ControlHeight;
        public double ModelId;

        // Events associated with the adorner to signal the main applicaton that something has changed
        public static readonly RoutedEvent OnAdornerModifiedEvent = EventManager.RegisterRoutedEvent("UpdateModelRequired", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlateCanvasControl));


        // expose our event
        public event RoutedEventHandler OnAdornerModified
        {
            add { AddHandler(OnAdornerModifiedEvent, value); }
            remove
            {
                RemoveHandler(OnAdornerModifiedEvent, value);
            }
        }


        public ResizeAdorner(UIElement adornedElement, int id, double scale=1.0) : base(adornedElement)
        {
            Rectangle rect = adornedElement as Rectangle;
            ControlWidth = rect.Width;
            ControlHeight = rect.Height;
            ModelId = id;

            SCREEN_SCALE_FACTOR = scale;

            AdornerVisuals = new VisualCollection(this);

            thumb1 = new Thumb() { Background = Brushes.Coral, Height = 10, Width = 10 };
            thumb2 = new Thumb() { Background = Brushes.Coral, Height = 10, Width = 10 };
            platecanvas_thumb1 = new Thumb() { Background = Brushes.Coral, Height = 10, Width = 10 };
            platecanvas_thumb2 = new Thumb() { Background = Brushes.Coral, Height = 10, Width = 10 };
            Rec = new Rectangle() { Stroke = Brushes.Coral, StrokeThickness = 2, StrokeDashArray = { 3, 2 }, Fill=Brushes.Transparent, IsHitTestVisible=false };
            txtBlock_VertDim = new TextBlock() { Text = "VertiDim", FontSize= 20, IsHitTestVisible = false };
            txtBlock_HorizDim = new TextBlock() { Text = "HorizDim", FontSize = 20, IsHitTestVisible = false };
            txtBlock_PlateNum = new TextBlock() { Text = "PlateNumber", FontSize = 40, IsHitTestVisible = false };

            thumb1.DragDelta += Thumb1_DragDelta;
            thumb2.DragDelta += Thumb2_DragDelta;

            platecanvas_thumb1.DragDelta += PlateCanvas_Thumb1_DragDelta;
            platecanvas_thumb2.DragDelta += PlateCanvas_Thumb2_DragDelta;


            AdornerVisuals.Add(Rec);
            AdornerVisuals.Add(thumb1);
            AdornerVisuals.Add(thumb2);
            AdornerVisuals.Add(txtBlock_VertDim);
            AdornerVisuals.Add(txtBlock_HorizDim);
            AdornerVisuals.Add(txtBlock_PlateNum);

        }

        private void PlateCanvas_Thumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var ele = (FrameworkElement)AdornedElement;

            // Change the height of the control and the rectangle inside of the control
            ele.Height = ele.Height - e.VerticalChange < 0 ? 0 : ele.Height - e.VerticalChange;

            // Change the height of the control and the rectangle inside of the control
            ele.Width = ele.Width - e.HorizontalChange < 0 ? 0 : ele.Width - e.HorizontalChange;

            ControlWidth = ele.Width;
            ControlHeight = ele.Height;

            RaiseEvent(new RoutedEventArgs(ResizeAdorner.OnAdornerModifiedEvent));



        }

        private void PlateCanvas_Thumb1_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var ele = (FrameworkElement)AdornedElement;

            ele.Height = ele.Height + e.VerticalChange < 0 ? 0 : ele.Height + e.VerticalChange;

            ele.Width = ele.Width + e.HorizontalChange < 0 ? 0 : ele.Width + e.HorizontalChange;

            ControlWidth = ele.Width;
            ControlHeight = ele.Height;

            RaiseEvent(new RoutedEventArgs(ResizeAdorner.OnAdornerModifiedEvent));


        }

        private void Thumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var ele = (FrameworkElement)AdornedElement;

            ele.Height = ele.Height + e.VerticalChange < 0 ? 0 : ele.Height + e.VerticalChange;

            ele.Width = ele.Width + e.HorizontalChange < 0 ? 0 : ele.Width + e.HorizontalChange;

            ControlWidth = ele.Width;
            ControlHeight = ele.Height;

            RaiseEvent(new RoutedEventArgs(ResizeAdorner.OnAdornerModifiedEvent));
        }

        private void Thumb1_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var ele = (FrameworkElement)AdornedElement;

            ele.Height = ele.Height - e.VerticalChange < 0 ? 0 : ele.Height - e.VerticalChange;
            ele.Width = ele.Width - e.HorizontalChange < 0 ? 0 : ele.Width - e.HorizontalChange;

            ControlWidth = ele.Width;
            ControlHeight = ele.Height;

            RaiseEvent(new RoutedEventArgs(ResizeAdorner.OnAdornerModifiedEvent));
        }

        protected override Visual GetVisualChild(int index)
        {
            return AdornerVisuals[index];
        }

        protected override int VisualChildrenCount => AdornerVisuals.Count;

        /// <summary>
        /// This draws the elements of the adorner.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Rec.Arrange(new Rect(-2.5, -2.5, AdornedElement.DesiredSize.Width + 5, AdornedElement.DesiredSize.Height + 5));
            thumb1.Arrange(new Rect(-5, -5, 10, 10));
            thumb2.Arrange(new Rect(AdornedElement.DesiredSize.Width - 5, AdornedElement.DesiredSize.Height - 5, 10, 10));

            // plate number label
            txtBlock_PlateNum.Text = ModelId.ToString();
            txtBlock_PlateNum.Arrange(
                new Rect(0,
                    0,
                    50,
                    50)
                );

            //horizontal dimension
            txtBlock_HorizDim.Text = ((ControlWidth) / SCREEN_SCALE_FACTOR).ToString();
            //            txtBlock_Horiz.Text = ((AdornedElement.DesiredSize.Width + 5 + 2.5) / SCREEN_SCALE_FACTOR).ToString();
            txtBlock_HorizDim.Arrange(new Rect(0.45 * (AdornedElement.DesiredSize.Width + 5 + 2.5), 0, 50, 50));

            // vertical dimension
            txtBlock_VertDim.Text = ((ControlHeight)/SCREEN_SCALE_FACTOR).ToString();
            //txtBlock_Vert.Text = ((AdornedElement.DesiredSize.Height + 5 + 2.5)/SCREEN_SCALE_FACTOR).ToString();
            txtBlock_VertDim.Arrange(new Rect(0, 0.45 * (AdornedElement.DesiredSize.Height + 5 + 2.5), 50, 50));

            return base.ArrangeOverride(finalSize);
        }
    }
}
