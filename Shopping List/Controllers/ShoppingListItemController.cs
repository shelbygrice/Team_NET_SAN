﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopping_List.Models;
using Shopping_List.Data;

namespace Shopping_List.Controllers
{
    public class ShoppingListItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShoppingListItem
        public ActionResult Index()
        {
            return View(db.ShoppingListItems.ToList());
        }

        // GET: ShoppingListItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);
            if (shoppingListItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingListItem);
        }

        //POST: UpdateCheckBox
        [HttpPost]
        //[ValidateAntiForgeryToken]  //referencing id in order to update IsChecked,creating a new instance of class and calling it "shoppingListItem"
        public ActionResult UpdateCheckbox([Bind(Include = "Id, IsChecked")] ShoppingListItem shoppingListItem)
        {   //pulling data from db and holding it in memory
            var item = db.ShoppingListItems.Find(shoppingListItem.Id);
            //referencing IsChecked on item and converting it to IsChecked on shoppingListItem
            item.IsChecked = shoppingListItem.IsChecked;
            //Save changes
            db.SaveChanges();
            return Json("success");
        }

        // GET: ShoppingListItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShoppingListItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ShoppingListId,Contents,IsChecked,CreatedUtc,ModifiedUtc")] ShoppingListItem shoppingListItem, int shoppingListId)
        {
            if (ModelState.IsValid)
            {
                //shoppingListItem.ShoppingListId = 26;
                db.ShoppingListItems.Add(shoppingListItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shoppingListItem);
        }
        // GET: ShoppingListItem/Edit/5
        public ActionResult Edit(int? id)
            {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);
            if (shoppingListItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingListItem);
            }

        // POST: ShoppingListItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShoppingListId,Contents,IsChecked,CreatedUtc,ModifiedUtc")] ShoppingListItem shoppingListItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingListItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shoppingListItem);
        }

        // GET: ShoppingListItem/Delete/5
        public ActionResult Delete(int? id) /*bool? saveChangesError = false*/
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //if (saveChangesError.GetValueOrDefault())
            //{
            //    ViewBag.ErrorMessage = "Delete failed.";
            //}
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);
            if (shoppingListItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingListItem);
        }

        // POST: ShoppingListItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingListItem shoppingListItem = db.ShoppingListItems.Find(id);

            using (var context = new ApplicationDbContext())
            {
                var query = from n in context.ShoppingListItems
                            where n.Id == id
                            select n.ShoppingListId;
                var list = query.ToList();
                int listId = list.ElementAt(0);


                db.ShoppingListItems.Remove(shoppingListItem);
                db.SaveChanges();

                return RedirectToAction("ViewItem", "ShoppingList", new { id = listId });
            }
                
            
            
        }

        //GET Clear Item?
        public ActionResult ClearItem()
        {
            return View(db.ShoppingListItems.Where(i => i.IsDeleted == true));
        }

        //POST Clear Item
        [HttpPost, ActionName("ClearItem")]
        [ValidateAntiForgeryToken]
        public ActionResult ClearAllItems()
        {
            IEnumerable<ShoppingListItem> ShoppingListItem = db.ShoppingListItems.Where(i => i.IsDeleted == true);
            foreach (var Item in ShoppingListItem)
            {
                db.ShoppingListItems.Remove(Item);
            }
            db.SaveChanges();

            return RedirectToAction("ViewItem");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
