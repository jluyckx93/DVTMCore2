using DVTMCore2.Models;
using DVTMCore2.Services;
using DVTMCore2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTMCore2.Controllers.Web
{
    public class AppController: Controller
    {
        private IConfigurationRoot _config;
        private IWorldRepository _repository;
        private IMailService _mailService;

        public AppController(IMailService mailService, IConfigurationRoot config, IWorldRepository repo)
        {
            _mailService = mailService;
            _config = config;
            _repository = repo;
 
        }
       public IActionResult Index()
        {
            return View();
        }

        public IActionResult Countries()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {

            if (model.Email.Contains("aol.com"))
            {
                ModelState.AddModelError("", "AOL is not supported!");
            }
            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAdress"], model.Email, "From Contact Page", model.Message);

                ModelState.Clear();
                ViewBag.UserMessage = "Message sent.";
            }
           


            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
