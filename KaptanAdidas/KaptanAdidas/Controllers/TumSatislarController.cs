using KaptanAdidas.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KaptanAdidas.Controllers
{
    public class TumSatislarController : Controller
    {
        // GET: TümSatislar
        KaptansporAdidasEntities2 db = new KaptansporAdidasEntities2();
        [Authorize(Roles = "A")]
        public ActionResult Index(int sayfa=1)
        {
            return View(db.Satislar.ToList().ToPagedList(sayfa,5));
        }
    }
}