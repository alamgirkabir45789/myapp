using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace myportfolio.Helpers
{
    public class FileSave
    {
        public static string SaveImage(out string fileName, IFormFile file, string localPath)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            string message = "success";
            
            if (file.Length > 2097152)
                message = "Select jpg or jpeg or png less than 2 ΜB.";

            var extention = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extention.ToLower()))
                message = "Must be jpeg or jpg or png";

            fileName = localPath + "/" + DateTime.Now.Ticks.ToString() + extention;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception)
            {
                message = "can not upload image";
            }
            return message;
        }

        public static string SaveImageAndVideo(out string fileName, IFormFile file, string localPath)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" ,".mp4", ".webm" };
            string message = "success";

            //if (file.Length > 2097152)
            //    message = "Select jpg or jpeg or png less than 2 ΜB.";

            var extention = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extention.ToLower()))
                message = "Must be jpeg or jpg or png";

            fileName = localPath + "/" + DateTime.Now.Ticks.ToString() + extention;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception e)
            {
                message = "can not upload image";
            }
            return message;
        }

        public static string SaveLargeImage(out string fileName, IFormFile file, string localPath)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            string message = "success";

            var extention = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extention.ToLower()))
                message = "Must be jpeg or jpg or png";

            fileName = localPath + "/" + DateTime.Now.Ticks.ToString() + extention;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception e)
            {
                message = "can not upload image";
            }
            return message;
        }

        public static string SaveDocument(out string fileName, IFormFile file, string localPath)
        {
            var allowedExtensions = new[] { ".pdf", ".ppt", ".pptx" };
            string message = "success";

            var extention = Path.GetExtension(file.FileName);
            
            if (!allowedExtensions.Contains(extention.ToLower()))
            {
                message = "Must be pdf or ppt or pptx";
            }
            
            fileName = localPath + "/" + DateTime.Now.Ticks.ToString() + extention;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception e)
            {
                message = "can not upload image";
            }
            return message;
        }

       


        public static string SaveVideo(out string fileName, IFormFile file, string localPath)
        {
            var allowedExtensions = new[] { ".mp4" };
            string message = "success";

            var extention = Path.GetExtension(file.FileName);

            if (!allowedExtensions.Contains(extention.ToLower()))
            {
                message = "Must be mp4";
            }

            fileName = localPath + "/" + DateTime.Now.Ticks.ToString() + extention;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception e)
            {
                message = "can not upload image";
            }
            return message;
        }
    }
}
