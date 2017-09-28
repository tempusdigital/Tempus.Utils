namespace Tempus.Utils.AspNetCore
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class HtmlExtensions
    {
        public static string GetQueryParameter(this IHtmlHelper htmlHelper, string parametroNome)
        {
            var resultado = htmlHelper.ViewContext.HttpContext.Request.Query[parametroNome].ToString();

            if (string.IsNullOrWhiteSpace(resultado))
            {
                resultado = htmlHelper.ViewContext.RouteData.Values[parametroNome]?.ToString();
            }

            return resultado ?? "";
        }

        public static int? GetQueryParameterInt(this IHtmlHelper htmlHelper, string parametroNome)
        {
            if (int.TryParse(htmlHelper.GetQueryParameter(parametroNome), out var parametro))
                return parametro;

            return null;
        }
    }
}
