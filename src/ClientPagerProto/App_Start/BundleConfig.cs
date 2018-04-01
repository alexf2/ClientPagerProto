using System;
using System.Web.Hosting;
using System.Web.Optimization;
using System.Text.RegularExpressions;

namespace ClientPagerProto
{
    public class BundleConfig
    {
        const string JqueryFileName = "~/Scripts/jquery*.js";

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            BundleTable.EnableOptimizations = true;

            var bandle = new ScriptBundle("~/bundles/jquery",
                string.Format("http://code.jquery.com/jquery-{0}.min.js", GetScriptVer(JqueryFileName))).
                Include("~/Scripts/jquery-{version}.js");
            bandle.CdnFallbackExpression = "window.jQuery";
            bundles.Add(bandle);

            bandle = new ScriptBundle("~/bundles/bootstrap", "http://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.bundle.min.js").
                Include("~/Scripts/bootstrap.bundle.js");
            bundles.Add(bandle);

            bandle = new ScriptBundle("~/bundles/jquery-ui", "http://code.jquery.com/ui/1.12.1/jquery-ui.min.js").
                Include("~/Scripts/jquery-ui-1.12.1.js");
            bundles.Add(bandle);

            bundles.Add(new ScriptBundle("~/bundles/category-list-js").Include(
                    "~/Scripts/jquery.browser.js",
                    "~/customScripts/jquery-helpers.js",
                    "~/customScripts/toolbox-mgr-plugin.js",
                    "~/customScripts/paginal-list-plugin.js",
                    "~/customScripts/paginal-list-option-plugin.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/normalize.css",
                      "~/customCss/site.css",
                      "~/Content/bootstrap.css"
            ));

            bundles.Add(new StyleBundle("~/Content/category-list-css").Include(
                "~/Content/normalize.css",
                "~/customCss/site.css",
                "~/customCss/paginal-list-plugin.css",
                "~/Content/jquery-ui*"
            ));

        }


        static readonly Regex _exParseVer = new Regex(@"\d+\.(\d+\.)*", RegexOptions.CultureInvariant | RegexOptions.Singleline);        

        static string GetScriptVer(string filePattern)
        {                        
            if (!BundleTable.VirtualPathProvider.FileExists(filePattern))
            {
                VirtualFile f = BundleTable.VirtualPathProvider.GetFile(filePattern);

                Match m = _exParseVer.Match(f.Name);
                if (!m.Success)
                    throw new Exception(string.Format("Can't extract version: {0}", f.Name));

                string res = m.Groups[0].Value;

                return res.EndsWith(".") ? res.Substring(0, res.Length - 1) : res;
            }
            else
            {
                throw new Exception(string.Format("Can't find {0}", filePattern));
            }            
        }
    }
}
