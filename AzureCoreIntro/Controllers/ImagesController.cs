using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureCoreIntro.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureCoreIntro
{
    [Route("[Controller]/[action]")]
    public class ImagesController : Controller
    {
        private readonly ImageStore imageStore;

        public ImagesController(ImageStore imageStore)
        {
            this.imageStore = imageStore;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile image)
        {
            if (image != null)
            {
                using (var stream = image.OpenReadStream())
                {
                    var imageId = await imageStore.SaveImage(stream);

                    return RedirectToAction("Show", new { imageId });
                }
            }
            return View();
        }

        [HttpGet("{imageId}")]
        public ActionResult Show(string imageId)
        {
            var model = new ShowModel { Uri = imageStore.UriFor(imageId) };
            return View(model);
        }
    }
}