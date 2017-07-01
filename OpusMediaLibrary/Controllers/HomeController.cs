using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpusMediaLibrary.Models;
using Dropbox.Api;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Globalization;

namespace OpusMediaLibrary.Controllers
{
    public class HomeController : Controller
    {
        OpusMediaContext db = new OpusMediaContext();

        // GET: Home
        public ActionResult Index()
        {
            var songs = db.Songs.Where(x=>x.CloudToken.Inactive == false).ToList();
            return View(songs);
        }

        public ActionResult Create()
        {
            ViewBag.storage =  db.CloudTokens.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.AccountName }).ToList();
            return View();
        }

        [HttpPost]
        public  ActionResult UploadAudioFile(HttpPostedFileBase audioFile, Song song, string BitRate)
        {
            if (audioFile.ContentLength > 0)
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                song.Title = textInfo.ToTitleCase(song.Title);
                song.Artist = textInfo.ToTitleCase(song.Artist);
                db.Songs.Add(song);
                song.OrigFilename = Path.GetFileName(audioFile.FileName);
                song.DateAdded = DateTime.Now;
                db.SaveChanges();

                string _FileName = song.Title + " - " + song.Artist + " [" + song.Id + "]." + Path.GetExtension(audioFile.FileName);
                string _path = Path.Combine(Server.MapPath("~/TempUpload"), _FileName);
                audioFile.SaveAs(_path);
                var fileToUpload = ConvertToOpus(_path, BitRate);
                if (System.IO.File.Exists(_path))
                {
                    System.IO.File.Delete(_path);
                }
                var task = Task.Run(() => UploadToDropbox(fileToUpload, song.Id, song.CloudTokenId));
                task.Wait();
            }
            try
            {   
               
            }
            catch
            {
                TempData["Message"] = "File upload failed!!";
            }
            return RedirectToAction("Index");
        }

        private string ConvertToOpus(string inputFile, string BitRate)
        {
            string outputFile = Path.Combine(Server.MapPath("~/TempUpload"), Path.GetFileNameWithoutExtension(inputFile) + ".opus");
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

            ffMpeg.ConvertMedia(inputFile,
                null, // autodetect by input file extension 
                outputFile,
                null, // autodetect by output file extension 
                new NReco.VideoConverter.ConvertSettings()
                {
                    CustomOutputArgs = " -acodec libopus -b:a " + BitRate + " -vbr on -compression_level 10 "
                }
            );

            return outputFile;
        }

        private async Task UploadToDropbox(string localPath, int songId, int CloudTokenId)
        {
            var token = db.CloudTokens.Find(CloudTokenId).Token;
            var song = db.Songs.Find(songId);
            using (var dbx = new DropboxClient(token))
            {
                var remotePath = "/Apps/OpusMediaLibrary/" + Path.GetFileName(localPath);
                using (var fileStream = System.IO.File.Open(localPath, FileMode.Open))
                {
                    var s = await dbx.Files.UploadAsync(remotePath, body: fileStream);
                    var result = await dbx.Sharing.CreateSharedLinkWithSettingsAsync(remotePath);
                    song.Link = result.Url;
                    db.Entry(song).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            if (System.IO.File.Exists(localPath))
            {
                System.IO.File.Delete(localPath);
            }
        }


    }
}