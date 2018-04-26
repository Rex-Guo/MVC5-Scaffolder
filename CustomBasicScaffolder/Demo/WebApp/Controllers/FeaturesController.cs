﻿#region Using

using System.Web.Mvc;

#endregion

namespace WebApp.Controllers
{
    [Authorize]
    public class FeaturesController : Controller
    {
        // GET: home/calendar
        public ActionResult Calendar()
        {
            return View();
        }

        // GET: home/google-map
        public ActionResult GoogleMap()
        {
            return View();
        }
        public ActionResult AgileBoard()
        {
            return View();
        }
        public ActionResult General() {
            return View();
        }
        public ActionResult Templates() {
            return View();
        }
        public ActionResult TeamMember()
        {
            return View();
        }
        public ActionResult Bootstrap() {
            return View();
        }
        public ActionResult Widgets() {
            return View();
        }
        public ActionResult Validator() {
            return View();
        }
    }
}