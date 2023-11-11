using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationWithImageAndIcon.Models;
using WebApplicationWithImageAndIcon.Models.ViewModels;
using PagedList;
using System.Web.Services.Description;
using System.Data.Entity;

namespace WebApplicationWithImageAndIcon.Controllers
{    
    public class PlayersController : Controller
    {
        private readonly SportsDbContext db = new SportsDbContext();
        public ActionResult Index(string searchString, int page = 1)
        {
            IQueryable<Player> players = db.Players.Include("Sports");

            if (!string.IsNullOrEmpty(searchString))
            {
                players = players.Where(p => p.PlayerName.Contains(searchString));
            }
            return View(players.OrderBy(x => x.PlayerId).ToPagedList(page, 5));
        }
        public PartialViewResult Search(string searchString, Grade? searchGrade)
        {
            IQueryable<Player> players = db.Players.Include("Sports");

            if (!string.IsNullOrEmpty(searchString))
            {
                players = players.Where(p => p.PlayerName.Contains(searchString));
            }

            if (searchGrade != null)
            {
                players = players.Where(p => p.PlayerGrade == searchGrade);
            }

            return PartialView("_SearchResult", players.ToList());
        }


        public ActionResult Create()
        {
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(PlayerViewModel pvm)
        {
            if(ModelState.IsValid)
            {
                if(pvm.Picture != null)
                {
                    // Image save
                    string filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName));
                    pvm.Picture.SaveAs(Server.MapPath(filePath));

                    // for player
                    Player player = new Player
                    {
                        PlayerName = pvm.PlayerName,
                        JoinDate = pvm.JoinDate,
                        PlayerGrade = pvm.PlayerGrade,
                        SportsId = pvm.SportsId,
                        PicturePath = filePath
                    };
                    db.Players.Add(player);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName");
            return View(pvm);
        }
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if(player == null)
            {
                return HttpNotFound();
            }
            PlayerViewModel pvm = new PlayerViewModel
            {
                PlayerId = player.PlayerId,
                PlayerName = player.PlayerName,
                JoinDate = player.JoinDate,
                PlayerGrade = player.PlayerGrade,
                SportsId = player.SportsId,
                PicturePath = player.PicturePath
            };
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName");
            return View(pvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlayerViewModel pvm)
        {
            if (ModelState.IsValid)
            {
                Player player = db.Players.Find(pvm.PlayerId);

                if (player == null)
                {
                    return HttpNotFound();
                }

                if (pvm.Picture != null && pvm.Picture.ContentLength > 0)
                {
                    // Image file exists, save and update the path
                    string filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName));
                    pvm.Picture.SaveAs(Server.MapPath(filePath));

                    player.PicturePath = filePath;
                }

                player.PlayerName = pvm.PlayerName;
                player.JoinDate = pvm.JoinDate;
                player.PlayerGrade = pvm.PlayerGrade;
                player.SportsId = pvm.SportsId;

                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName", pvm.SportsId);
            return View(pvm);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }

            return View(player);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}