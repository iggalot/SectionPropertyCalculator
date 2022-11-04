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
        public PlateModel Model { get; set; }

        public double SetTop { get; set; }
        public double SetLeft { get; set; }

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
        public PlateViewModel(PlateModel model)
        {
            Model = model;
            SetLeft = model.Centroid.X;
            SetTop = model.Centroid.Y;
        }
    }
}
