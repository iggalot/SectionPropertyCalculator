using SectionPropertyCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SectionPropertyCalculator.ViewModels
{
    public class PlateViewModel : BaseViewModel
    {
        public Canvas cCanvas { get; set; }
        public PlateModel Model { get; set; }

        /// <summary>
        /// Canvas coordinates for the plate view model
        /// </summary>
        public double SetTop { get => cCanvas.Height * 0.5 - 0.5 * Model.Height; }
        public double SetLeft { get => cCanvas.Width * 0.5 - 0.5 * Model.Width; }

        /// <summary>
        /// Finds the color to draw the material fill
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public Brush GetMaterialColor(MaterialTypes type)
        {
            switch (type)
            {
                case MaterialTypes.MATERIAL_UNDEFINED:
                    return Brushes.Cyan;
                case MaterialTypes.MATERIAL_STEEL:
                    return Brushes.Red;
                case MaterialTypes.MATERIAL_WOOD_SYP:
                    return Brushes.Blue;
                case MaterialTypes.MATERIAL_WOOD_DF:
                    return Brushes.Blue;
                case MaterialTypes.MATERIAL_WOOD_LVL_E2_0:
                    return Brushes.Green;
                default:
                    throw new System.ArgumentException("In GetMaterialColor: " + type.ToString() + " error");
            }
        }
        public PlateViewModel(PlateModel model, Canvas c)
        {
            Model = model;
            cCanvas = c;
            //SetLeft = model.Centroid.X - 0.5 * model.Width;
            //SetTop = model.Centroid.Y - 0.5 * model.Height;
        }
    }
}
