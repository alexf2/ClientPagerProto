﻿// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
#pragma warning disable 1591, 3008, 3009
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
public static partial class MVC
{
    public static ClientPagerProto.Controllers.DemoListController DemoList = new ClientPagerProto.Controllers.T4MVC_DemoListController();
    public static ClientPagerProto.Controllers.HomeController Home = new ClientPagerProto.Controllers.T4MVC_HomeController();
    public static T4MVC.SharedController Shared = new T4MVC.SharedController();
}

namespace T4MVC
{
}

namespace T4MVC
{
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class Dummy
    {
        private Dummy() { }
        public static Dummy Instance = new Dummy();
    }
}

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
internal partial class T4MVC_System_Web_Mvc_ActionResult : System.Web.Mvc.ActionResult, IT4MVCActionResult
{
    public T4MVC_System_Web_Mvc_ActionResult(string area, string controller, string action, string protocol = null): base()
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }
     
    public override void ExecuteResult(System.Web.Mvc.ControllerContext context) { }
    
    public string Controller { get; set; }
    public string Action { get; set; }
    public string Protocol { get; set; }
    public RouteValueDictionary RouteValueDictionary { get; set; }
}



namespace Links
{
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class Scripts {
        private const string URLPATH = "~/Scripts";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        public static readonly string _references_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/_references.min.js") ? Url("_references.min.js") : Url("_references.js");
        public static readonly string bootstrap_bundle_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/bootstrap.bundle.min.js") ? Url("bootstrap.bundle.min.js") : Url("bootstrap.bundle.js");
        public static readonly string bootstrap_bundle_js_map = Url("bootstrap.bundle.js.map");
        public static readonly string bootstrap_bundle_min_js = Url("bootstrap.bundle.min.js");
        public static readonly string bootstrap_bundle_min_js_map = Url("bootstrap.bundle.min.js.map");
        public static readonly string bootstrap_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/bootstrap.min.js") ? Url("bootstrap.min.js") : Url("bootstrap.js");
        public static readonly string bootstrap_js_map = Url("bootstrap.js.map");
        public static readonly string bootstrap_min_js = Url("bootstrap.min.js");
        public static readonly string bootstrap_min_js_map = Url("bootstrap.min.js.map");
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class esm {
            private const string URLPATH = "~/Scripts/esm";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string popper_utils_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/popper-utils.min.js") ? Url("popper-utils.min.js") : Url("popper-utils.js");
            public static readonly string popper_utils_js_map = Url("popper-utils.js.map");
            public static readonly string popper_utils_min_js = Url("popper-utils.min.js");
            public static readonly string popper_utils_min_js_map = Url("popper-utils.min.js.map");
            public static readonly string popper_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/popper.min.js") ? Url("popper.min.js") : Url("popper.js");
            public static readonly string popper_js_map = Url("popper.js.map");
            public static readonly string popper_min_js = Url("popper.min.js");
            public static readonly string popper_min_js_map = Url("popper.min.js.map");
        }
    
        public static readonly string index_d_ts = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/index.d.min.js") ? Url("index.d.min.js") : Url("index.d.js");
        public static readonly string jquery_3_3_1_intellisense_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery-3.3.1.intellisense.min.js") ? Url("jquery-3.3.1.intellisense.min.js") : Url("jquery-3.3.1.intellisense.js");
        public static readonly string jquery_3_3_1_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery-3.3.1.min.js") ? Url("jquery-3.3.1.min.js") : Url("jquery-3.3.1.js");
        public static readonly string jquery_3_3_1_min_js = Url("jquery-3.3.1.min.js");
        public static readonly string jquery_3_3_1_min_map = Url("jquery-3.3.1.min.map");
        public static readonly string jquery_3_3_1_slim_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery-3.3.1.slim.min.js") ? Url("jquery-3.3.1.slim.min.js") : Url("jquery-3.3.1.slim.js");
        public static readonly string jquery_3_3_1_slim_min_js = Url("jquery-3.3.1.slim.min.js");
        public static readonly string jquery_3_3_1_slim_min_map = Url("jquery-3.3.1.slim.min.map");
        public static readonly string jquery_helpers_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery-helpers.min.js") ? Url("jquery-helpers.min.js") : Url("jquery-helpers.js");
        public static readonly string jquery_ui_1_12_1_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery-ui-1.12.1.min.js") ? Url("jquery-ui-1.12.1.min.js") : Url("jquery-ui-1.12.1.js");
        public static readonly string jquery_ui_1_12_1_min_js = Url("jquery-ui-1.12.1.min.js");
        public static readonly string jquery_browser_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.browser.min.js") ? Url("jquery.browser.min.js") : Url("jquery.browser.js");
        public static readonly string jquery_browser_min_js = Url("jquery.browser.min.js");
        public static readonly string paginal_list_option_plugin_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/paginal-list-option-plugin.min.js") ? Url("paginal-list-option-plugin.min.js") : Url("paginal-list-option-plugin.js");
        public static readonly string paginal_list_plugin_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/paginal-list-plugin.min.js") ? Url("paginal-list-plugin.min.js") : Url("paginal-list-plugin.js");
        public static readonly string popper_utils_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/popper-utils.min.js") ? Url("popper-utils.min.js") : Url("popper-utils.js");
        public static readonly string popper_utils_js_map = Url("popper-utils.js.map");
        public static readonly string popper_utils_min_js = Url("popper-utils.min.js");
        public static readonly string popper_utils_min_js_map = Url("popper-utils.min.js.map");
        public static readonly string popper_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/popper.min.js") ? Url("popper.min.js") : Url("popper.js");
        public static readonly string popper_js_map = Url("popper.js.map");
        public static readonly string popper_min_js = Url("popper.min.js");
        public static readonly string popper_min_js_map = Url("popper.min.js.map");
        public static readonly string README_md = Url("README.md");
        public static readonly string respond_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/respond.min.js") ? Url("respond.min.js") : Url("respond.js");
        public static readonly string respond_matchmedia_addListener_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/respond.matchmedia.addListener.min.js") ? Url("respond.matchmedia.addListener.min.js") : Url("respond.matchmedia.addListener.js");
        public static readonly string respond_matchmedia_addListener_min_js = Url("respond.matchmedia.addListener.min.js");
        public static readonly string respond_min_js = Url("respond.min.js");
        public static readonly string toolbox_mgr_plugin_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/toolbox-mgr-plugin.min.js") ? Url("toolbox-mgr-plugin.min.js") : Url("toolbox-mgr-plugin.js");
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class umd {
            private const string URLPATH = "~/Scripts/umd";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string popper_utils_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/popper-utils.min.js") ? Url("popper-utils.min.js") : Url("popper-utils.js");
            public static readonly string popper_utils_js_map = Url("popper-utils.js.map");
            public static readonly string popper_utils_min_js = Url("popper-utils.min.js");
            public static readonly string popper_utils_min_js_map = Url("popper-utils.min.js.map");
            public static readonly string popper_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/popper.min.js") ? Url("popper.min.js") : Url("popper.js");
            public static readonly string popper_js_map = Url("popper.js.map");
            public static readonly string popper_min_js = Url("popper.min.js");
            public static readonly string popper_min_js_map = Url("popper.min.js.map");
        }
    
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class Content {
        private const string URLPATH = "~/Content";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        public static readonly string bootstrap_grid_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/bootstrap-grid.min.css") ? Url("bootstrap-grid.min.css") : Url("bootstrap-grid.css");
             
        public static readonly string bootstrap_grid_css_map = Url("bootstrap-grid.css.map");
        public static readonly string bootstrap_grid_min_css = Url("bootstrap-grid.min.css");
        public static readonly string bootstrap_grid_min_css_map = Url("bootstrap-grid.min.css.map");
        public static readonly string bootstrap_reboot_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/bootstrap-reboot.min.css") ? Url("bootstrap-reboot.min.css") : Url("bootstrap-reboot.css");
             
        public static readonly string bootstrap_reboot_css_map = Url("bootstrap-reboot.css.map");
        public static readonly string bootstrap_reboot_min_css = Url("bootstrap-reboot.min.css");
        public static readonly string bootstrap_reboot_min_css_map = Url("bootstrap-reboot.min.css.map");
        public static readonly string bootstrap_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/bootstrap.min.css") ? Url("bootstrap.min.css") : Url("bootstrap.css");
             
        public static readonly string bootstrap_css_map = Url("bootstrap.css.map");
        public static readonly string bootstrap_min_css = Url("bootstrap.min.css");
        public static readonly string bootstrap_min_css_map = Url("bootstrap.min.css.map");
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class images {
            private const string URLPATH = "~/Content/images";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string button_bg_jpg = Url("button-bg.jpg");
            public static readonly string ui_bg_diagonals_thick_18_b81900_40x40_png = Url("ui-bg_diagonals-thick_18_b81900_40x40.png");
            public static readonly string ui_bg_diagonals_thick_20_666666_40x40_png = Url("ui-bg_diagonals-thick_20_666666_40x40.png");
            public static readonly string ui_bg_flat_10_000000_40x100_png = Url("ui-bg_flat_10_000000_40x100.png");
            public static readonly string ui_bg_glass_100_f6f6f6_1x400_png = Url("ui-bg_glass_100_f6f6f6_1x400.png");
            public static readonly string ui_bg_glass_100_fdf5ce_1x400_png = Url("ui-bg_glass_100_fdf5ce_1x400.png");
            public static readonly string ui_bg_glass_65_ffffff_1x400_png = Url("ui-bg_glass_65_ffffff_1x400.png");
            public static readonly string ui_bg_gloss_wave_35_f6a828_500x100_png = Url("ui-bg_gloss-wave_35_f6a828_500x100.png");
            public static readonly string ui_bg_highlight_soft_100_eeeeee_1x100_png = Url("ui-bg_highlight-soft_100_eeeeee_1x100.png");
            public static readonly string ui_bg_highlight_soft_75_ffe45c_1x100_png = Url("ui-bg_highlight-soft_75_ffe45c_1x100.png");
            public static readonly string ui_icons_222222_256x240_png = Url("ui-icons_222222_256x240.png");
            public static readonly string ui_icons_228ef1_256x240_png = Url("ui-icons_228ef1_256x240.png");
            public static readonly string ui_icons_ef8c08_256x240_png = Url("ui-icons_ef8c08_256x240.png");
            public static readonly string ui_icons_ffd27a_256x240_png = Url("ui-icons_ffd27a_256x240.png");
            public static readonly string ui_icons_ffffff_256x240_png = Url("ui-icons_ffffff_256x240.png");
        }
    
        public static readonly string jquery_ui_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery-ui.min.css") ? Url("jquery-ui.min.css") : Url("jquery-ui.css");
             
        public static readonly string jquery_ui_min_css = Url("jquery-ui.min.css");
        public static readonly string jquery_ui_structure_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery-ui.structure.min.css") ? Url("jquery-ui.structure.min.css") : Url("jquery-ui.structure.css");
             
        public static readonly string jquery_ui_structure_min_css = Url("jquery-ui.structure.min.css");
        public static readonly string jquery_ui_theme_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery-ui.theme.min.css") ? Url("jquery-ui.theme.min.css") : Url("jquery-ui.theme.css");
             
        public static readonly string jquery_ui_theme_min_css = Url("jquery-ui.theme.min.css");
        public static readonly string normalize_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/normalize.min.css") ? Url("normalize.min.css") : Url("normalize.css");
             
        public static readonly string paginal_list_plugin_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/paginal-list-plugin.min.css") ? Url("paginal-list-plugin.min.css") : Url("paginal-list-plugin.css");
             
        public static readonly string site_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/site.min.css") ? Url("site.min.css") : Url("site.css");
             
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class themes {
            private const string URLPATH = "~/Content/themes";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
            public static class @base {
                private const string URLPATH = "~/Content/themes/base";
                public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
                public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
                public static readonly string accordion_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/accordion.min.css") ? Url("accordion.min.css") : Url("accordion.css");
                     
                public static readonly string all_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/all.min.css") ? Url("all.min.css") : Url("all.css");
                     
                public static readonly string autocomplete_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/autocomplete.min.css") ? Url("autocomplete.min.css") : Url("autocomplete.css");
                     
                public static readonly string base_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/base.min.css") ? Url("base.min.css") : Url("base.css");
                     
                public static readonly string button_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/button.min.css") ? Url("button.min.css") : Url("button.css");
                     
                public static readonly string core_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/core.min.css") ? Url("core.min.css") : Url("core.css");
                     
                public static readonly string datepicker_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/datepicker.min.css") ? Url("datepicker.min.css") : Url("datepicker.css");
                     
                public static readonly string dialog_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/dialog.min.css") ? Url("dialog.min.css") : Url("dialog.css");
                     
                public static readonly string draggable_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/draggable.min.css") ? Url("draggable.min.css") : Url("draggable.css");
                     
                [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
                public static class images {
                    private const string URLPATH = "~/Content/themes/base/images";
                    public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
                    public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
                    public static readonly string ui_bg_flat_0_aaaaaa_40x100_png = Url("ui-bg_flat_0_aaaaaa_40x100.png");
                    public static readonly string ui_bg_flat_75_ffffff_40x100_png = Url("ui-bg_flat_75_ffffff_40x100.png");
                    public static readonly string ui_bg_glass_55_fbf9ee_1x400_png = Url("ui-bg_glass_55_fbf9ee_1x400.png");
                    public static readonly string ui_bg_glass_65_ffffff_1x400_png = Url("ui-bg_glass_65_ffffff_1x400.png");
                    public static readonly string ui_bg_glass_75_dadada_1x400_png = Url("ui-bg_glass_75_dadada_1x400.png");
                    public static readonly string ui_bg_glass_75_e6e6e6_1x400_png = Url("ui-bg_glass_75_e6e6e6_1x400.png");
                    public static readonly string ui_bg_glass_95_fef1ec_1x400_png = Url("ui-bg_glass_95_fef1ec_1x400.png");
                    public static readonly string ui_bg_highlight_soft_75_cccccc_1x100_png = Url("ui-bg_highlight-soft_75_cccccc_1x100.png");
                    public static readonly string ui_icons_222222_256x240_png = Url("ui-icons_222222_256x240.png");
                    public static readonly string ui_icons_2e83ff_256x240_png = Url("ui-icons_2e83ff_256x240.png");
                    public static readonly string ui_icons_444444_256x240_png = Url("ui-icons_444444_256x240.png");
                    public static readonly string ui_icons_454545_256x240_png = Url("ui-icons_454545_256x240.png");
                    public static readonly string ui_icons_555555_256x240_png = Url("ui-icons_555555_256x240.png");
                    public static readonly string ui_icons_777620_256x240_png = Url("ui-icons_777620_256x240.png");
                    public static readonly string ui_icons_777777_256x240_png = Url("ui-icons_777777_256x240.png");
                    public static readonly string ui_icons_888888_256x240_png = Url("ui-icons_888888_256x240.png");
                    public static readonly string ui_icons_cc0000_256x240_png = Url("ui-icons_cc0000_256x240.png");
                    public static readonly string ui_icons_cd0a0a_256x240_png = Url("ui-icons_cd0a0a_256x240.png");
                    public static readonly string ui_icons_ffffff_256x240_png = Url("ui-icons_ffffff_256x240.png");
                }
            
                public static readonly string jquery_ui_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery-ui.min.css") ? Url("jquery-ui.min.css") : Url("jquery-ui.css");
                     
                public static readonly string jquery_ui_min_css = Url("jquery-ui.min.css");
                public static readonly string menu_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/menu.min.css") ? Url("menu.min.css") : Url("menu.css");
                     
                public static readonly string progressbar_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/progressbar.min.css") ? Url("progressbar.min.css") : Url("progressbar.css");
                     
                public static readonly string resizable_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/resizable.min.css") ? Url("resizable.min.css") : Url("resizable.css");
                     
                public static readonly string selectable_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/selectable.min.css") ? Url("selectable.min.css") : Url("selectable.css");
                     
                public static readonly string selectmenu_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/selectmenu.min.css") ? Url("selectmenu.min.css") : Url("selectmenu.css");
                     
                public static readonly string slider_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/slider.min.css") ? Url("slider.min.css") : Url("slider.css");
                     
                public static readonly string sortable_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/sortable.min.css") ? Url("sortable.min.css") : Url("sortable.css");
                     
                public static readonly string spinner_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/spinner.min.css") ? Url("spinner.min.css") : Url("spinner.css");
                     
                public static readonly string tabs_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/tabs.min.css") ? Url("tabs.min.css") : Url("tabs.css");
                     
                public static readonly string theme_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/theme.min.css") ? Url("theme.min.css") : Url("theme.css");
                     
                public static readonly string tooltip_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/tooltip.min.css") ? Url("tooltip.min.css") : Url("tooltip.css");
                     
            }
        
        }
    
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static partial class Bundles
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static partial class Scripts {}
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static partial class Styles {}
    }
}

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
internal static class T4MVCHelpers {
    // You can change the ProcessVirtualPath method to modify the path that gets returned to the client.
    // e.g. you can prepend a domain, or append a query string:
    //      return "http://localhost" + path + "?foo=bar";
    private static string ProcessVirtualPathDefault(string virtualPath) {
        // The path that comes in starts with ~/ and must first be made absolute
        string path = VirtualPathUtility.ToAbsolute(virtualPath);
        
        // Add your own modifications here before returning the path
        return path;
    }

    // Calling ProcessVirtualPath through delegate to allow it to be replaced for unit testing
    public static Func<string, string> ProcessVirtualPath = ProcessVirtualPathDefault;

    // Calling T4Extension.TimestampString through delegate to allow it to be replaced for unit testing and other purposes
    public static Func<string, string> TimestampString = System.Web.Mvc.T4Extensions.TimestampString;

    // Logic to determine if the app is running in production or dev environment
    public static bool IsProduction() { 
        return (HttpContext.Current != null && !HttpContext.Current.IsDebuggingEnabled); 
    }
}





#endregion T4MVC
#pragma warning restore 1591, 3008, 3009


