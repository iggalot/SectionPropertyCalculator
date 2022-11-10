using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SectionPropertyCalculator.Helpers
{
    /// <summary>
    /// Helpers for WPF visual tree
    /// </summary>
    public static class CChildHelper
    {
        /// <summary>
        /// Detaches child from parent.  Does not delete child, so can move child under another element OK.  
        /// </summary>
        /// <example>
        /// //As static method.
        /// DependencyObject p = VisualTreeHelper.GetParent(myObj);
        /// CChildHelper.DisconnectChild(p, myObj); //Disconnects myObj.  
        /// 
        /// //Or as extension.
        /// DependencyObject p = VisualTreeHelper.GetParent(myObj);
        /// p.DisconnectChild(myObj);
        /// 
        public static void DisconnectChild(this DependencyObject parent, UIElement child)
        {
            // Grid, StackPanel, Canvas inherit from Panel.  So this is most common
            var panel = parent as Panel;
            if(panel != null)
            {
                panel.Children.Remove(child);  // Remove disconnects, does not delete
                return;
            }

            var decorator = parent as Decorator;
            if(decorator != null)
            {
                if(decorator.Child == child)
                {
                    decorator.Child = null;
                }
                return;
            }

            var contentPresenter = parent as ContentPresenter;
            if(contentPresenter != null)
            {
                if(contentPresenter.Content == child)
                {
                    contentPresenter.Content = null;
                }
                return;
            }

            //Window inherits from ContentControl
            var contentControl = parent as ContentControl;
            if(contentControl != null)
            {
                if(contentControl.Content == child)
                {
                    contentControl.Content = null;
                }
                return;
            }

            // maybe more needed here?
        }

        public static void AddChild(this DependencyObject parent, UIElement child)
        {
            // Grid, StackPanel, Canvas inherit from Panel.  So this is most common
            var panel = parent as Panel;
            if (panel != null)
            {
                panel.Children.Add(child);
                return;
            }

            var decorator = parent as Decorator;
            if (decorator != null)
            {
                decorator.AddChild(child);
                return;
            }

            var contentPresenter = parent as ContentPresenter;
            if (contentPresenter != null)
            {
                contentPresenter.AddChild(child);
            }

            //Window inherits from ContentControl
            var contentControl = parent as ContentControl;
            if (contentControl != null)
            {
                contentControl.AddChild(child);
                return;
            }

            // maybe more needed here?
        }
    }
}
