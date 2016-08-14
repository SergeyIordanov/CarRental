using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CarRental.WEB.Helpers
{
    public static class DropDownList
    {
        public static MvcHtmlString CreateDropDown(this HtmlHelper html, string header, Dictionary<string, string> options, object htmlAttributes = null)
        {
            TagBuilder select = new TagBuilder("select");

            TagBuilder option = new TagBuilder("option");
            option.SetInnerText(header);
            option.Attributes.Add(new KeyValuePair<string, string>("value", ""));
            //option.Attributes.Add(new KeyValuePair<string, string>("disabled", "disabled"));
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