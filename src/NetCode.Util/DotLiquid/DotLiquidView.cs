/* Copyright (C) 2012 by Matt Brailsford

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DotLiquid.FileSystems;
using DotLiquid.ViewEngine.FileSystems;
using DotLiquid.ViewEngine.Tags;
using DotLiquid.ViewEngine.Util;

namespace DotLiquid.ViewEngine
{
    public class DotLiquidView : IView
    {
        private ControllerContext _controllerContext;

        public string MasterPath { get; protected set; }
        public string ViewPath { get; protected set; }

        public DotLiquidView(ControllerContext controllerContext,
            string partialPath)
            : this(controllerContext, partialPath, null)
        { }

        public DotLiquidView(ControllerContext controllerContext,
            string viewPath, string masterPath)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            if (string.IsNullOrEmpty(viewPath))
                throw new ArgumentNullException("viewPath");

            _controllerContext = controllerContext;

            ViewPath = viewPath;
            MasterPath = masterPath;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            if (viewContext == null)
                throw new ArgumentNullException("viewContext");

            // Copy data from the view context over to DotLiquid
            var localVars = new Hash();

            if (viewContext.ViewData.Model != null)
            {
                var model = viewContext.ViewData.Model;

                //if(model.GetType().Name.EndsWith("ViewModel"))
                //{
                //    // If it's view model, just copy all properties to the localVars collection
                //    localVars.Merge(Hash.FromAnonymousObject(model));
                //}
                //else
                //{
                // It it's not a view model, just add the model direct as a "model" variable
                localVars.Add("model", model);
                //}
            }

            foreach (var item in viewContext.ViewData)
                localVars.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);

            foreach (var item in viewContext.TempData)
                localVars.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);

            var renderParams = new RenderParameters
            {
                LocalVariables = Hash.FromDictionary(localVars)
            };

            // Render the template
            var fileContents = VirtualPathProviderHelper.Load(ViewPath);
            var template = Template.Parse(fileContents);
            template.Render(writer, renderParams);
        }
    }
}
