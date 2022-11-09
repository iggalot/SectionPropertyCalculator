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
        public List<PlateViewModel> lstPlateViewModel { get; set; } = new List<PlateViewModel>();

        public string PlateInfoString
        {
            get
            {
                return GetPlateListString();
            }
        }


        public CompositeSectionViewModel(CompositeShapeModel model, Canvas c)
        {
            // Assemble the view models for all of the plates of the composite shape model
            CreateCompositePlateViewModel(model, c);
        }

        protected void CreateCompositePlateViewModel(CompositeShapeModel model, Canvas c)
        {
            lstPlateViewModel.Clear();

            // create the view models
            foreach (var item in model.Plates)
            {
                lstPlateViewModel.Add(new PlateViewModel(item, c));
            }
        }

        private string GetPlateListString()
        {
            string str = "";
            foreach (PlateViewModel plate_vm in lstPlateViewModel)
            {
                str += "\nID: " + plate_vm.Model.Id + "  W: " + plate_vm.Model.Width + "   H: " + plate_vm.Model.Height;
            }

            return str;
        }

        public void Update()
        {
            OnPropertyChanged("PlateInfoString");
        }


    }
}
