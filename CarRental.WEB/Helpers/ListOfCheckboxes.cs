﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CarRental.WEB.Helpers
{
    public static class ListOfCheckboxes
    {

        public static MvcHtmlString CreateCheckboxesList(this HtmlHelper html, string id, string propName, IEnumerable<string> values, object htmlAttributes = null)
        {
            int currentIndex = 0;

            var container = new TagBuilder("div");

            foreach (string item in values)
            {
                var par = new TagBuilder("p");
                par.Attributes.Add(new KeyValuePair<string, string>("class", "checkbox-par"));

                var checkbox = new TagBuilder("input");
                checkbox.Attributes.Add(new KeyValuePair<string, string>("id", id + "-" + currentIndex));
                checkbox.Attributes.Add(new KeyValuePair<string, string>("name", propName));
                checkbox.Attributes.Add(new KeyValuePair<string, string>("type", "checkbox"));
                checkbox.Attributes.Add(new KeyValuePair<string, string>("value", item));
                par.InnerHtml += checkbox.ToString();

                var label = new TagBuilder("label");
                label.Attributes.Add(new KeyValuePair<string, string>("for", id + "-" + currentIndex));
                label.InnerHtml = item;
                par.InnerHtml += label.ToString();

                container.InnerHtml += par.ToString();
                currentIndex++;
            }

            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var props = type.GetProperties();

                Dictionary<string, string> dic = props.ToDictionary(x => x.Name, x => x.GetValue(htmlAttributes, null).ToString());

                foreach (var attr in dic)
                {
                    container.MergeAttribute(attr.Key, attr.Value);
                }
            }
            return new MvcHtmlString(container.ToString());
        }
    }
}