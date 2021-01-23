using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CustomerService;
using LoginService;
using Microsoft.AspNetCore.Mvc;
using Pos.Gateway.Securities.Common;
using Pos.WebApplication.Models;

namespace Pos.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly LoginServicce.LoginServicceClient _login;
        private readonly CustomerServicce.CustomerServicceClient _customer;
        public HomeController(
            LoginServicce.LoginServicceClient login,
            CustomerServicce.CustomerServicceClient customer)
        {
            _login = login;
            _customer = customer;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Account account)
         {

            // if validate fail
            if (!ModelState.IsValid)
            {
                TempData["LoginError"] = "Tài khoản và mật khẩu là bắt buộc";
                return Redirect("/");
            }

            var accountRequest = new LoginRequest
            {
                UserName = account.Username,
                Password = account.Password
            };

            var accountReuslt = await _login.LoginSystemAsync(accountRequest);
            if (accountReuslt.Status == GatewaySecureCommon.ErrorSystem)
            {
                TempData["LoginError"] = "Lỗi hệ thống";
                return Redirect("/");
            }
            if (accountReuslt.Status == GatewaySecureCommon.NotFound)
            {
                TempData["LoginError"] = "Sai tài khoản hoặc mật khẩu";
                return Redirect("/");
            }
            //string idUser = "858B5A7B-24A8-4C6C-BB7E-31F6A9E701CC";
            //var customerRequest = new GetCustomerRequest
            //{
            //    Id = idUser
            //};
            //var customerReuslt = await _customer.GetCustomerAsync(customerRequest);
            return View(accountReuslt);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
