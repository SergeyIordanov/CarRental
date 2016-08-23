using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CarRental.WEB.Helpers
{
    public static class DropDownList
    {
        /// <summary>
        /// Creates a drop-down list markup
        /// </summary>
        /// <param name="html">Extention base</param>
        /// <param name="header">Header of the list</param>
        /// <param name="activeHeader">Boolean: is header selectable</param>
        /// <param name="options">Options of the list. Dictionary: key - displayed text, value - value</param>
        /// <param name="htmlAttributes">Object with deffs of html attributes</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString CreateDropDown(this HtmlHelper html, string header, bool activeHeader, Dictionary<string, string> options, object htmlAttributes = null)
        {
            TagBuilder select = new TagBuilder("select");

            TagBuilder option = new TagBuilder("option");
            option.SetInnerText(header);
            option.Attributes.Add(new KeyValuePair<string, string>("value", ""));
            if(!activeHeader)
                option.Attributes.Add(new KeyValuePair<string, string>("disabled", "disabled"));
            option.Attributes.Add(new KeyValuePair<string, string>("selected", "selected"));
            select.InnerHtml += option.ToString();

            foreach (var item in options)
            {
                option = new TagBuilder("option");
                option.SetInnerText(item.Key);
                option.Attributes.Add(new KeyValuePair<string, string>("value", item.Value));
                select.InnerHtml += option.ToString();
            }

            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var props = type.GetProperties();

                Dictionary<string, string> dic = props.ToDictionary(x => x.Name, x => x.GetValue(htmlAttributes, null).ToString());

                foreach (var attr in dic)
                {
                    select.MergeAttribute(attr.Key, attr.Value);
                }
            }
            return new MvcHtmlString(select.ToString());
        }
    }
}