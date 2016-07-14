using CategoriesProductsAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace CategoriesProductsAPI.Controllers
{
    [EnableCors("*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        private CategoriesManagerVM db;
        #region "Categories"
        public CategoriesController()
        {
            db = Retrieve();
        }

        [ResponseType(typeof(CategoriesManagerVM))]
        public CategoriesManagerVM GetCategories()
        {

            return db;
        }
        #endregion

        #region "Categories GetById "
        [ResponseType(typeof(categoryVm))]
        public IHttpActionResult GetCategory(int id)
        {
            categoryVm Vm = db.categories.Where(_Cat => _Cat.categoryID == id).SingleOrDefault();

            if ((Vm == null))
            {
                return null;
            }
            return Ok(Vm);
        }
        #endregion

        #region "Categories Post"
        // POST api/CategoryVm
        [ResponseType(typeof(categoryVm))]
        public IHttpActionResult PostCategory(categoryVm vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newID = db.categories.Max(c => c.categoryID)+1;
            vm.categoryID = newID;
            db.categories.Add(vm);
            WriteData(db.categories);
            return Ok(vm);
        }
        #endregion

        #region "Categories Put"
        // PUT api/Categories/5
        [ResponseType(typeof(categoryVm))]
        public IHttpActionResult PutCategory(int id, categoryVm vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            categoryVm Vm = db.categories.Where(_Cat => _Cat.categoryID ==id ).SingleOrDefault();
            Vm.categoryName = vm.categoryName;
            Vm.description = vm.description;
            WriteData(db.categories);

            return Ok(vm);
        }
        #endregion

        #region "Categories Delete"
        //  DELETE api/Categories/5
        [ResponseType(typeof(categoryVm))]
        public IHttpActionResult DeleteCategory(int id)
        {
            categoryVm Vm = db.categories.Where(_Cat => _Cat.categoryID == id).SingleOrDefault();
            db.categories.Remove(Vm);


            if (Vm == null)
            {
                return NotFound();
            }
            WriteData(db.categories);

            return Ok(Vm);
        }
        #endregion

        #region "Json Manager"
        private void WriteData(List<categoryVm> categories)
        {
            var filePath = HostingEnvironment.MapPath("~/App_Data/categories.json");
            var json = JsonConvert.SerializeObject(categories, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, json);
        }
        internal CategoriesManagerVM Retrieve()
        {
            var filePathCategories = HostingEnvironment.MapPath("~/App_Data/categories.json");
            var filePathProducts = HostingEnvironment.MapPath("~/App_Data/products.json");

            var jsonCategories = System.IO.File.ReadAllText(filePathCategories);
            var jsonProducts = System.IO.File.ReadAllText(filePathProducts);

            CategoriesManagerVM results = new CategoriesManagerVM();

            results.categories = JsonConvert.DeserializeObject<List<categoryVm>>(jsonCategories);
            results.products = JsonConvert.DeserializeObject<List<productVm>>(jsonProducts);

            return results;
        }
        #endregion
    }
}
