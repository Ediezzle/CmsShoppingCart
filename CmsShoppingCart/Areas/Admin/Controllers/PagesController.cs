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
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //checked model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db= new Db())
            {
                //Declare slug
                string slug;

                //Initialise PageDTO
                PageDTO dto = new PageDTO();

                //DTO title
                dto.Title = model.Title;

                //Check for and set slug if need be
                if(string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }

                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists!");
                    return View(model);
                }

                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSideBar = model.HasSideBar;
                dto.Sorting = 100;

                //Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();

                //Set TempData message
                TempData["SuccessMessage"] = "You have added a new page!";

                //Redirect
                return RedirectToAction("AddPage");
            }
        }

        // GET: Admin/Pages/EditPage/id
        //if you dont specify Http method it is "Get" by Default
        public ActionResult EditPage(int id)
        {
            //Declare PageVM
            PageVM model;

            using (Db db = new Db())
            {

                //Get the page
                PageDTO dto = db.Pages.Find(id);

            //Confirm page exists
            if (dto == null)
                {
                    return Content("The page does not exist!");
                }

                //Initialise PageVM
                model = new PageVM(dto);

                //Return view with model
                return View(model);
            }
        }

        // POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //get page id
                int id = model.Id;

                //declare and initialise slug
                string slug = "home";

                //get the page
                PageDTO dto = db.Pages.Find(id);

                //DTO title
                dto.Title = model.Title;

                //check for slug and set it if need be
                if(model.Slug != "home")
                {
                    if(string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }

                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                //make sure the title and slug are unique
                if(db.Pages.Where(x=>x.Id!=id).Any(x=>x.Title == model.Title) || db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists!");
                    return View(model);
                }

                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSideBar = model.HasSideBar;

                //save the DTO
                db.SaveChanges();

            }

            //set the TempData message
            TempData["SuccessMessage"] = "You have successfully edited the page!";

            //redirect
            return RedirectToAction("EditPage");
            
        }

        // GET: Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            //declare PageVM
            PageVM model;

            using (Db db = new Db())
            {

                //get the page
                PageDTO dto = db.Pages.Find(id);

                //confirm page exists
                if(dto==null)
                {
                    return Content("The page doesn't exist!");
                }

                //init the PageVM
                model = new PageVM(dto);

            }
            //return the view with model 
            return View(model);
        }

        // GET: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                //get page
                PageDTO dto = db.Pages.Find(id);

                //remove the page
                db.Pages.Remove(dto);

                //save changes
                db.SaveChanges();
                
            }
            return RedirectToAction("index");   
        }

        // POST: Admin/Pages/DeletePage/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using(Db db = new Db())
            {
                //set initial count
                int count = 1;

                //declare PageDTO
                PageDTO dto;

                //set sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }
           
        }

    }

    
}