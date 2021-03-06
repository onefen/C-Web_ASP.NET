﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarDealer.Data;
using CarDealer.Models.BindingModels;
using CarDealer.Models.EntityModels;
using CarDealer.Services;
using Microsoft.Ajax.Utilities;

namespace CarDealerApp.Controllers
{
    public class CustomersController : Controller
    {
        private CarDealerContext db = new CarDealerContext();
        private CustomerService service = new CustomerService(Data.Context);

        // GET: Customers

        [Route("~/customers/all")]
        public ActionResult Index()
        {
            IEnumerable<Customer> customers = this.db.Customers.ToList();
            return View(customers);
        }

        [Route("~/customers/all/{sorthType}")]
        public ActionResult Index(string sorthType)
        {
            IEnumerable<Customer> orderedCustomers = null;

            if (sorthType == "ascending")
            {
                orderedCustomers = this.db.Customers.OrderBy(c => c.BirthDate).ThenBy(c => c.IsYoungDriver);
            }

            else if (sorthType == "descending")
            {
                orderedCustomers = this.db.Customers.OrderByDescending(c => c.BirthDate).ThenBy(c => c.IsYoungDriver);
            }

            else
            {
                orderedCustomers = this.db.Customers;
            }

            return View(orderedCustomers.ToList());
        }

        [Route("~/customers")]
        public ActionResult TotalSales()
        {
            Customer customer = this.db.Customers.Find(1);
            return View(customer);
        }

        [Route("~/customers/{id:int}")]
        public ActionResult TotalSales(int id)
        {
            Customer customer = this.db.Customers.Find(id);
            return View(customer);
        }

        // GET: Customers/Create
        [Route("~/Customers/Add")]
        public ActionResult Add()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("~/Customers/Add")]
        public ActionResult Add([Bind(Include = "Name,BirthDate")] AddCustomerBindingModel addCustomerBindingModel)
        {
            if (this.ModelState.IsValid)
            {
                this.service.AddCustomer(addCustomerBindingModel);
                return this.RedirectToAction("Index");
            }

            return this.View();
        }

        [Route("~/Customers/Edit")]
        public ActionResult Edit()
        {
            EditCustomerBindingModel bind = this.service.GetEditedCustomer();
            return this.View(bind);
        }

        [Route("~/Customers/Edit/{id:int}")]
        public ActionResult Edit(int id)
        {
            EditCustomerBindingModel bind = this.service.GetEditedCustomer(id);
            return this.View(bind);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("~/Customers/Edit/{id:int}")]
        public ActionResult Edit([Bind(Include = "Id,Name,BirthDate")] EditCustomerBindingModel bind)
        {
            if (this.ModelState.IsValid)
            {
                this.service.EditCustomer(bind);
                return this.RedirectToAction("Index");
            }

            return this.View();
        }

        [Route("~/Customers/Delete")]
        public ActionResult Delete()
        {
            this.service.DeleteCustomer();
            return this.RedirectToAction("Index");
        }

        [Route("~/Customers/Delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            this.service.DeleteCustomer(id);
            return this.RedirectToAction("Index");
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
