using System.Collections.Generic;

namespace CategoriesProductsAPI.Models
{
    public class CategoriesManagerVM
    {
        public List<categoryVm> categories { get; set; }
        public List<productVm> products { get; set; }
    }
}
