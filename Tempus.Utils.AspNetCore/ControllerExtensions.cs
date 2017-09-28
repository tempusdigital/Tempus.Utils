namespace Tempus.Utils.AspNetCore
{
    using HeyRed.Mime;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;

    public static class ControllerExtensions
    {
        public static IActionResult DownloadFile(this Controller source, string filePath)
        {
            if (!File.Exists(filePath))
                return source.NotFound();

            var name = filePath.Split('\\').Last();

            var mimeType = MimeGuesser.GuessMimeType(filePath);

            var stream = new FileStream(filePath, FileMode.Open);

            return source.File(stream, mimeType, name);
        }

        public static ActionResult RedirectToActionJson<TController>(this TController source, string action)
            where TController : Controller
        {
            return source.JsonNet(new
            {
                redirect = source.Url.Action(action)
            });
        }

        public static ActionResult RedirectToActionJson<TController>(this TController source, string action, string controller)
            where TController : Controller
        {
            controller = FixControllerName(controller);

            return source.JsonNet(new
            {
                redirect = source.Url.Action(action, controller)
            });
        }

        public static ActionResult RedirectToActionJson<TController>(this TController source, string action, string controller, object values)
            where TController : Controller
        {
            controller = FixControllerName(controller);

            return source.JsonNet(new
            {
                redirect = source.Url.Action(action, controller, values)
            });
        }

        public static ActionResult RedirectToActionJson<TController>(this TController source, string action, object values)
            where TController : Controller
        {
            return source.JsonNet(new
            {
                redirect = source.Url.Action(action, values)
            });
        }

        private static string FixControllerName(string controller)
        {
            return controller?.Replace("Controller", "");
        }

        public static ContentResult JsonNet(this Controller controller, object model)
        {
            var serialized = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return new ContentResult
            {
                Content = serialized,
                ContentType = "application/json"
            };
        }
    }
}