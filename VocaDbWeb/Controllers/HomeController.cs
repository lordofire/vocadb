﻿using System;
using System.Web.Mvc;
using System.Web.SessionState;
using VocaDb.Model.Domain;
using VocaDb.Model.Domain.Globalization;
using VocaDb.Model.Domain.Security;
using VocaDb.Model.Service;
using VocaDb.Model.Service.BrandableStrings;
using VocaDb.Model.Service.Search;
using VocaDb.Web.Helpers;
using VocaDb.Web.Models;

namespace VocaDb.Web.Controllers
{

	[SessionState(SessionStateBehavior.ReadOnly)]
    public class HomeController : ControllerBase
    {

		private readonly BrandableStringsManager brandableStringsManager;
		private readonly OtherService otherService;
		private readonly SongService songService;
		private readonly UserService userService;

		public HomeController(SongService songService, OtherService otherService, UserService userService, BrandableStringsManager brandableStringsManager) {
			this.songService = songService;
			this.otherService = otherService;
			this.userService = userService;
			this.brandableStringsManager = brandableStringsManager;
		}

		public ActionResult Chat() {

			return View();

		}

		// Might still be used by some clients with opensearch
		[Obsolete("Moved to web api")]
		public ActionResult FindNames(string term) {

			var result = otherService.FindNames(SearchTextQuery.Create(term), 10);

			return Json(result);

		}

        //
        // GET: /Home/

        public ActionResult Index() {

			PageProperties.Description = brandableStringsManager.Home.SiteDescription;

			var contract = otherService.GetFrontPageContent(WebHelper.IsSSL(Request));

            return View(contract);

        }

		[HttpPost]
		public ActionResult GlobalSearch(GlobalSearchBoxModel model) {

			switch (model.ObjectType) {
				case EntryType.Undefined:
					return RedirectToAction("Index", "Search", new { filter = model.GlobalSearchTerm });

				case EntryType.Album:
					return RedirectToAction("Index", "Search", new { filter = model.GlobalSearchTerm, searchType = model.ObjectType });

				case EntryType.Artist:
					return RedirectToAction("Index", "Search", new { filter = model.GlobalSearchTerm, searchType = model.ObjectType });

				case EntryType.Song:
					return RedirectToAction("Index", "Search", new { filter = model.GlobalSearchTerm, searchType = model.ObjectType });

				default:
					var controller = model.ObjectType.ToString();
					return RedirectToAction("Index", controller, new {filter = model.GlobalSearchTerm});

			}


		}

		public ActionResult PVContent(int songId = invalidId) {

			if (songId == invalidId)
				return NoId();

			var song = songService.GetSongWithPVAndVote(songId);

			return PartialView("PVs/_PVContent", song);

		}

		public ActionResult Search(string filter) {
			return RedirectToAction("Index", "Search", new { filter });
		}

		public ActionResult Wiki() {
			return View();
		}

    }
}
