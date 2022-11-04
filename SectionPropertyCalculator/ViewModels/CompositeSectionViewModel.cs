using SectionPropertyCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SectionPropertyCalculator.ViewModels
{
    public class CompositeSectionViewModel : BaseViewModel
    {
        public CompositeShapeModel CompShapeModel { get; set; } = new CompositeShapeModel();

        public List<PlateViewModel> lstPlateViewModel { get; set; } = new List<PlateViewModel>();

        public CompositeSectionViewModel(CompositeShapeModel model)
        {
            CompShapeModel = model;

            lstPlateViewModel.Clear();

            // create the view models
            foreach(var item in CompShapeModel.Plates)
            {
                PlateViewModel view_model = new PlateViewModel(item);
                lstPlateViewModel.Add(view_model);
            }
        }

        public void Add(PlateViewModel plate_view_model)
        {
            lstPlateViewModel.Add(plate_view_model);
        }
    }
}
