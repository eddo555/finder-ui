﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static finder_ui.SessionHandler.CustomAuthorization;

namespace finder_ui.Controllers
{
    //[Authorize]
    [CustomAuthorization]
    public class MessageController : Controller

    {
        MessageServiceReference.Service1Client messageClient = new MessageServiceReference.Service1Client();
        Group3ServiceReference.Service1Client adClient = new Group3ServiceReference.Service1Client();

        UserLoginServiceReference.LoginServiceClient getUsers = new UserLoginServiceReference.LoginServiceClient();

        public ActionResult Index()
        {
            int sessId = Convert.ToInt32(Session["UserId"]); //parsar sessionId till int
            IEnumerable<MessageServiceReference.Messageinfo> messageList = messageClient.GetMessages().ToList();
            

            ViewBag.userMedd = messageClient.GetUserMessage(sessId); //hämtar ens egna meddelanden
            ViewBag.Lista = messageList.Where(x=>x.SendingUserId==sessId);// visar lista på endast egna meddelanden

            //ViewBag.annonsClient = adClient.AdvancedSearch(sessId); //funkar inte att skicka in id, måste skicka in 10 parametrar???

            //Group3ServiceReference.Service svar = adClient.GetServiceById(7); Jan olof  försök
            ViewBag.annonsMessage = adClient.GetServiceById(7); //hämtar service 7
            ViewBag.getUsers = getUsers.GetActiveUsers().ToList();
            ViewBag.getUser = getUsers.GetActiveUsers();

            ViewBag.test = getUsers.GetActiveUsers().Where(x => x.ID == sessId).ToList(); // test ger info om specifik active user


            return View();
        }

        [HttpPost]
        public ActionResult Index(MessageServiceReference.AddMessage newMessage)
        {
            MessageServiceReference.Service1Client messageClient = new MessageServiceReference.Service1Client();
            int sessId = Convert.ToInt32(Session["UserId"]);
            messageClient.AddMessage(newMessage, sessId);
            IEnumerable<MessageServiceReference.Messageinfo> messagelist = messageClient.GetMessages().ToList();
            ViewBag.Lista = messagelist;
            return RedirectToAction("index","message");
        }

        //ny vy med alla recievingUserId av ett slag, inparameter måste skapas i index vy
        public ActionResult Message (int id)
        {
            int sessId = Convert.ToInt32(Session["UserId"]); //parsar sessionId till int
            IEnumerable<MessageServiceReference.Messageinfo> messageList = messageClient.GetMessages().ToList();


            ViewBag.userMedd = messageClient.GetUserMessage(sessId); //hämtar ens egna meddelanden
            ViewBag.Lista = messageList.Where(x => x.SendingUserId == sessId && x.RecievingUserId== id);// visar lista på endast egna meddelanden

            return View();
        }
        [HttpPost]
        public ActionResult Message(MessageServiceReference.AddMessage newMessage, int id)
        {
            int sessId = Convert.ToInt32(Session["UserId"]); //parsar sessionId till int
            IEnumerable<MessageServiceReference.Messageinfo> messageList = messageClient.GetMessages().ToList();
            messageClient.AddMessage(newMessage, sessId);

            ViewBag.userMedd = messageClient.GetUserMessage(sessId); //hämtar ens egna meddelanden
            ViewBag.Lista = messageList.Where(x => x.SendingUserId == sessId && x.RecievingUserId == id);// visar lista på endast egna meddelanden

            return RedirectToAction("Message", "Message");
        }
        public ActionResult Search()
        {
            Group3ServiceReference.Service1Client search = new Group3ServiceReference.Service1Client();
            ViewBag.search = search.Search("hund").ToList();
            

            return View();
        }
        [HttpPost]
        public ActionResult Search(string a)
        {
            Group3ServiceReference.Service1Client search = new Group3ServiceReference.Service1Client();
            ViewBag.search = search.Search(a).ToList();


            return View();
        }

        public ActionResult Kontrakt (int serviceId, int counterpartId, int serviceOwnerId)
        {
            Group3ServiceReference.Service1Client skapaKontrakt = new Group3ServiceReference.Service1Client();
            ViewBag.skapaKontrakt = skapaKontrakt.CreateContract();

            return View();
        }

    }
}