using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using workForum.Models;

namespace workForum.Controllers
{
    public class HomeController : Controller
    {
        workForumContext db = new workForumContext();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

    public ActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Index(string Account, string Password)
    {
        var data = db.Member.Where(m => m.Account == Account && m.Password == Password).FirstOrDefault();
        if (data == null)
        {
            ViewBag.Message = "帳號密碼錯誤,登入失敗";
            return View();
        }
        HttpContext.Session.SetString("Account", data.Account);
        return RedirectToAction("showArticle"); //轉向至論壇
    }

        public ActionResult Photo()
        {
            string acc = HttpContext.Session.GetString("Account");
            var data = db.Album.Where(m => m.Account == acc).OrderByDescending(m => m.CreateTime).ToList();
            return View("Photo", data);
        }

        public IActionResult Delete(int id) { var data = db.Album.Where(m => m.Alb_Id == id).FirstOrDefault(); db.Album.Remove(data); db.SaveChanges(); return RedirectToAction("Photo"); }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile files)
        {

            if (files.Length > 0)
            {
                string filePath = "wwwroot/images/" + files.FileName;
                using (var stream = System.IO.File.Create(filePath))
                {
                    //程式寫入的本地資料夾裡面
                    await files.CopyToAsync(stream);
                }
                Album album = new Album();
                album.FileName = files.FileName;
                album.Url = "";
                album.Size = (int)files.Length;
                album.Type = "image";
                album.Account = HttpContext.Session.GetString("Account");
                album.CreateTime = DateTime.Now;
                db.Album.Add(album);
                db.SaveChanges();
            }
            return RedirectToAction("Photo");
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

        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult register(string Account, string Password, string Name, string Email, string AuthCode, bool isAdmin)
        {
            Member m = new Member();
            m.Account = Account;
            m.Password = Password;
            m.Name = Name;
            m.Email = Email;
            m.AuthCode = "";
            m.IsAdmin = isAdmin;
            db.Member.Add(m);
            db.SaveChanges();
            HttpContext.Session.SetString("Account", Account);
            return RedirectToAction("Photo");
        }

        public ActionResult Article()   //新增論壇
        {
            return View();
        }
        [HttpPost]
        public async Task <ActionResult> Article(string Title, string Content, IFormFile photo)
        {
            Article art = new Article();
            art.Title = Title;
            
            art.Content = Content;
            art.Account = HttpContext.Session.GetString("Account");
            art.CreateTime = DateTime.Now;
            art.Watch = 1;
            //上傳圖檔
            string fileName = "";
            //檔案上傳
            if (photo != null)
            {
                if (photo.Length > 0)
                {
                    string filePath = "wwwroot/images/" + photo.FileName;
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        //程式寫入的本地資料夾裡面
                        await photo.CopyToAsync(stream);
                    }
                }
                art.FileNAme = photo.FileName;
                db.Article.Add(art);
                db.SaveChanges();

            }

            return RedirectToAction("showArticle");
        }

        public ActionResult showArticle()
        {
            var data = db.Article.OrderByDescending(m => m.CreateTime).ToList();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult delete(int id)
        {
            var article = db.Article.Find(id);
            if (article != null)
            {
                try
                {
                    db.Article.Remove(article);
                    db.SaveChanges();
                    TempData["Message"] = "文章刪除成功！";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "刪除失敗：" + ex.Message;
                }
            }
            else
            {
                TempData["Message"] = "找不到這篇文章。";
            }

            return RedirectToAction("showArticle");
        }

        public ActionResult showBoard(int a_id)
        {
            var rec = db.Article.Where(m => m.A_Id == a_id).FirstOrDefault();
            rec.Watch += 1;   //瀏覽人次+1
            HttpContext.Session.SetString("title", rec.Title); //留言的標題
            db.SaveChanges();

            var data = db.Message.Where(m => m.A_Id == a_id).OrderByDescending(m => m.CreateTime).ToList();
            HttpContext.Session.SetString("a_id", a_id.ToString());
            return View(data);
        }

        public ActionResult Board()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Board(string content)
        {
            Message m = new Message();
            m.A_Id = Int32.Parse(HttpContext.Session.GetString("a_id"));
            m.Account = HttpContext.Session.GetString("Account");
            m.Content = content;
            m.CreateTime = DateTime.Now;
            db.Message.Add(m);
            db.SaveChanges();
            return RedirectToAction("showBoard", new { a_id = m.A_Id });
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        

    }
}
