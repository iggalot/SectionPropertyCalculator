using System;
using System.Collections.Generic;

namespace SectionPropertyCalculator.Models
{
    public class CompositeShapeModel : BaseModel
    {
        public List<PlateModel> Plates { get; set; } = new List<PlateModel>();

        public void AddPlate(PlateModel model)
        {
            if(model != null)
            {
                Plates.Add(model);
            }
        }

    }
}
