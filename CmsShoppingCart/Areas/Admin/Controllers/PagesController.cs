using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
          /*  var pagesList = new List<PageVM>{
                            new PageVM() { Id = 3, Title = "Home", Slug = "home", Body = "home page", Sorting = 3, HasSideBar = true }}; */


    //Declare a list of page VM
    List<PageVM> pagesList;


            using (Db db = new Db())
            {
                //Initialise the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            //Return view with list
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPage
        public ActionResult AddPage()
        {
            return View();
        }
    }
}