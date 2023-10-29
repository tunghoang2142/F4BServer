using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace F4BBuddyAppWebService.Controllers
{
    public class FileController : Controller
    {

        public FileController() { }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFile([FromBody]string data)
        {
            int id = int.Parse(HttpContext.User.Claims.First(c => c.Type.Equals("Id")).Value);
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string path = System.IO.Path.Combine(currentDirectory, "input");
            path = System.IO.Path.Combine(path, id + ".png");

            using var stream = System.IO.File.Create(path);
            stream.Write(Convert.FromBase64String(data), 0, data.Length);
            return Ok();
        }
    }
}