using KaptanAdidas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace KaptanAdidas.Controllers
{
    public class TumSatislar1Controller : Controller
    {
        // GET: TumSatislar1
        KaptansporAdidasEntities6 db = new KaptansporAdidasEntities6();
        [Authorize(Roles ="A")]
        public ActionResult Index(int sayfa=1)
        {
            return View(db.Satislar.ToList().ToPagedList(sayfa,5));
        }
    }
}