﻿using System.Web.Mvc;
using System.Web.Security;
using GameShop.Services;
using GameShop.ViewModels;
using GameShop.DataAccess;
using GameShop.Models;

namespace GameShop.Controllers
{
    public class AccountController : Controller
    {
        private AccountService _accountService;

        public AccountController()
        {
            _accountService = new AccountService();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (_accountService.Login(model.Login, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Login, true);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Имя пользователя и пароль были введены неверно. Либо ваш пользователь не зарегистрирован.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        private GameShopContext db = new GameShopContext();

        // GET: Customers/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Customers/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,FirstName,LastName,Birthday,Login,Password")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();
        }
    }
}