using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TitleAttribute : Attribute
    {
        public TitleAttribute(string Title)
        {
            this.DisplayTitle = Title;
        }
        public string DisplayTitle { get; set; }
    }
}