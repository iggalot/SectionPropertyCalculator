using SectionPropertyCalculator.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SectionPropertyCalculator.Controls.Adorners
{
    public class CResizeAdorner : Adorner
    {
        /// <summary>
        /// Element that can resize
        /// </summary>
        public FrameworkElement adornedElement;

        /// <summary>
        /// Auto-generated element. Wraps adornedElement and provides reference position during resize
        /// </summary>
        public Canvas parentCanvas = new Canvas();

        /// <summary>
        /// Application window.  Class finds this automatically.
        /// </summary>
        public Window window_;

        /// <summary>
        /// Max height for the adorned element.  In pixels.
        /// </summary>
        public double maxHeightRR;

        /// <summary>
        /// MAx width for the adorned element. In pixels.
        /// </summary>
        public double maxWidthRR;

        /// <summary>
        /// Resizing adorner uses Thumbs for visual elements.  Thumbs have built-in mouse input handling.
        /// </summary>
        public Thumb bottomRight = new Thumb();

        public VisualCollection visualChildren;

        /// <summary>
        /// Enables resizing on an element.
        /// Adds resize handle to bottom right corner.
        /// </summary>
        /// <example>
        /// Very simple one-line call.
        /// new CResizeAdorner(myButton, 200, 200); // Allow resizing myButton to max 200px.
        /// </example>
        /// <param name="_adornedElement">Element to apply resize ability to.</param>
        /// <param name="maxHeight_">Max height allowed when resizing. In pixels</param>
        /// <param name="maxWidth_">Max width allowed when resizing. In pixels</param>
        public CResizeAdorner(FrameworkElement _adornedElement, double maxHeight_, double maxWidth_) : base(_adornedElement) // Needs this "base" snipper when extending Adorner class
        {
            if (_adornedElement.IsLoaded == false)
            {
                MessageBox.Show("Error CResizeAdorner: Element " + _adornedElement.Name + " not loaded. Exiting. ");
                return;
            }

            // Keep this on top
            visualChildren = new VisualCollection(this);

            //From constructor
            adornedElement = _adornedElement;
            maxHeightRR = maxHeight_;
            maxWidthRR = maxWidth_;
            window_ = Window.GetWindow(adornedElement);

            //Set parent canvas size equal to resize element.
            parentCanvas.Height = adornedElement.RenderSize.Height; ;
            parentCanvas.Width = adornedElement.RenderSize.Width;

            //Debug only, to view canvas
//            parentCanvas.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#faaaaa"));
            parentCanvas.Opacity = 1.0;  // Making invisible is OK.  This value cumulative adds each time the control is renewed for some reason

            // Move resize element inside parent canvas
            DependencyObject parentOld = VisualTreeHelper.GetParent(adornedElement);
            parentOld.DisconnectChild(adornedElement);  // Element cannot have two parents
            parentOld.AddChild(parentCanvas);
            parentCanvas.AddChild(adornedElement);

            //Transfer alignment from adornedElement into parentCanvas
            parentCanvas.HorizontalAlignment = adornedElement.HorizontalAlignment;
            parentCanvas.VerticalAlignment = adornedElement.VerticalAlignment;

            //Transfer margin settings (left, right, top, down) from adornedElement into parentCanvas
            parentCanvas.Margin = new Thickness(
                adornedElement.Margin.Left,
                adornedElement.Margin.Top,
                adornedElement.Margin.Right,
                adornedElement.Margin.Bottom);

            //Remove margins from child, otherwise it will offset from parent canvas.
            adornedElement.Margin = new Thickness(0, 0, 0, 0);

            //Set adorned Element position to upper left.  This is required for resizing (gives it an anchor to reference).  Prevents both sides from shrinking when resizing element with mouse
//            Canvas.SetTop(adornedElement, 0.0);
//            Canvas.SetLeft(adornedElement, 0.0);

            //Apply layer to element
            AdornerLayer aLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            aLayer.Add(this);

            //Create corner icon
            BuildAdornerCorner();

            //Handles resizing event.
            bottomRight.DragDelta += new DragDeltaEventHandler(HandleResizing);

            //Stop resizing event.
            bottomRight.DragCompleted += delegate
            {
                //ShrinkCanvas back to element's size
                parentCanvas.Height = adornedElement.RenderSize.Height;
                parentCanvas.Width = adornedElement.RenderSize.Width;
                parentCanvas.Opacity = 1.0;
            };

        }

        /// <summary>
        /// Handler for resizing from the bottom right
        /// </summary>
        /// <param name="sender">From event</param>
        /// <param name="e">From event</param>
        public void HandleResizing(object sender, DragDeltaEventArgs args)
        {
            Thumb hitThumb = sender as Thumb;

            //First set minimum values
            //RenderSize is best.  Native Height/Width occasionally returns weird Nan values (not want).  DesiredSize includes margin sizes (don't want)
            var heightResize = Math.Max(adornedElement.RenderSize.Height + args.VerticalChange, hitThumb.RenderSize.Height);
            var widthResize = Math.Max(adornedElement.RenderSize.Width + args.HorizontalChange, hitThumb.RenderSize.Width);

            //Second, set max values.
            //Also resizes element.
            adornedElement.Height = Math.Min(heightResize, maxHeightRR);
            adornedElement.Width = Math.Min(widthResize, maxWidthRR);

            // Resize parent canvas so it has room to grow
            parentCanvas.Height = adornedElement.RenderSize.Height + 100;  //Need at least 60px for fast mouse moves.  100px hadnels very fast moves
            parentCanvas.Width = adornedElement.RenderSize.Width + 100;  //Need at least 60px for fast mouse moves.  100px hadnels very fast moves
        }

        /// <summary>
        /// Moves drag icon to correct position.
        /// </summary>
        public void PositionThumb()
        {
            //RenderSize is best.  Native Height/Width occasionally returns weird Nan values (not want).  DesiredSize includes margin sizes (don't want)
            var elemHeight = adornedElement.RenderSize.Height;
            var elemWidth = adornedElement.RenderSize.Width;

            // Fiddled with this to get it lined up perfectly.  Places thumb in elemet's lower right corner.
            bottomRight.Arrange(new Rect(
               (elemWidth-bottomRight.Width) / 2,      // Placement
               (elemHeight - bottomRight.Height) / 2,     // Placement
               elemWidth,                                 //Size
               elemHeight                                 //Size
               ));
        }

        /// <summary>
        /// Mainly to hook into "arrange elements on screen" event.
        /// </summary>
        /// <param name="finalSize">Not used, only defined for override method</param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // Position the thumb icon
            PositionThumb();

            // Return the final size.  Not used but need for this override.
            return finalSize;
        }

        /// <summary>
        /// Creates drag icon and set cursor property
        /// </summary>
        public void BuildAdornerCorner()
        {
            // Grab xaml file that draws a little triangle.
            ResourceDictionary myDictionary = Application.LoadComponent(new Uri("/SectionPropertyCalculator;component/ResourceDictionaries/CResizeAdornerTriangle.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;
            bottomRight.Style = (Style)myDictionary["CResizingAdorner_ThumbTriangle"];

            // Visual characteristics
            bottomRight.Cursor = Cursors.SizeNWSE;
            bottomRight.Height = 14;    // If updating size, also update CResizeAdornerTriangle.xaml
            bottomRight.Width = 14;     // If updating size, also update CResizeAdornerTriangle.xaml
            bottomRight.Opacity = 1.0;

            visualChildren.Add(bottomRight);
        }

        // These are needed.  Override the VisualChildrenCount and GetVisualChild properties to interface
        // with the adorner's visual collection.
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }
}
