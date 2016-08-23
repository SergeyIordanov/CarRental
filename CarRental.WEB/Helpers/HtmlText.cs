using System.Web.Mvc;

namespace CarRental.WEB.Helpers
{
    public static class HtmlText
    {
        /// <summary>
        /// Writes text to the page like html
        /// </summary>
        /// <param name="html">Extension base</param>
        /// <param name="text">Text to write</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString WriteHtml(this HtmlHelper html, string text)
        {
            return new MvcHtmlString(text);
        }
    }
}