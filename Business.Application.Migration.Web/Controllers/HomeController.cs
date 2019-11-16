﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Microsoft.Teams.Samples.TaskModule.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("configure")]
        public ActionResult Configure()
        {
            return View();
        }

        [Route("CreateItem")]
        public ActionResult CreateItem(Category category)
        {
            ViewBag.Result = "Create Item";
            if (Request.HttpMethod.ToUpperInvariant() == "POST")
            {
                var item = new ItemInfo()
                {
                    Id = Guid.NewGuid(),
                    Name = Request.Form["name"],
                    Description = Request.Form["description"],
                    Link = Request.Form["link"],
                    Image = string.IsNullOrEmpty(Request.Form["image"]) ? "http://lorempixel.com/800/800?rand=" + DateTime.Now.Ticks.ToString() : Request.Form["image"],
                    CreatedUser = Request.Form["createdUser"],
                    Category = (Category)Enum.Parse(typeof(Category), Request.Form["category"], true),
                    CreatedTime = DateTime.Now,
                };
                ItemDB.AddItem(item);
            }
            ViewBag.Category = category;
            return View(ViewBag);
        }

        [Route("UpdateItem/{id}")]
        public ActionResult UpdateItem(Guid id)
        {
            ViewBag.Result = "Update Item";
            var item = ItemDB.Items.Find(b => b.Id == id);
            if (Request.HttpMethod.ToUpperInvariant() == "POST")
            {
                item.Name = Request.Form["name"];
                item.Description = Request.Form["description"];
                item.Link = Request.Form["link"];
                item.Image = string.IsNullOrEmpty(Request.Form["image"]) ? "http://lorempixel.com/800/800?rand=" + DateTime.Now.Ticks.ToString() : Request.Form["image"];
                item.UpdatedUser = Request.Form["updatedUser"];
                item.UpdatedTime = DateTime.Now;
            }

            return View(item);
        }

        [Route("ListItem")]
        public ActionResult ListItem(Category category = Category.All)
        {
            var token = Request.Cookies.Get("token")?.Value;
            if (string.IsNullOrEmpty(token) && category == Category.All)
            {
                return RedirectToAction("Login");
            }

            List<ItemInfo> result;
            if (category == Category.All)
            {
                if (Request.HttpMethod == "POST")
                {
                    var keyword = Request.Form["searchBox"];
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        result = ItemDB.SearchItems(keyword);
                    }
                    else
                        result = ItemDB.SearchItems(keyword);
                }
                else
                    result = ItemDB.Items;
            }
            else
            {
                result = ItemDB.Items.Where(i => i.Category == category).ToList();
            }
            ViewBag.Category = category;
            return View(result);
        }

        [Route("Login")]
        public ActionResult Login()
        {
            if (Request.HttpMethod == "POST")
            {
                var token = ItemDB.SignIn(Request.Form["name"], Request.Form["password"]);
                var tokenCookie = new HttpCookie("token", token);
                tokenCookie.Expires.AddDays(7);
                HttpContext.Response.Cookies.Add(tokenCookie);
                return RedirectToAction("ListItem", new { category = Category.All });
            }
            return View();
        }
    }
}
