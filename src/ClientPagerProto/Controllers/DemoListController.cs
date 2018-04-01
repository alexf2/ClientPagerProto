using System.Web.Mvc;
using System.Web.UI;
using ClientPagerProto.DataSource;
using ClientPagerProto.DataSource.Viking;
using ClientPagerProto.Models;
using ClientPagerProto.PaggedList;
using CqsDataFoundation.Query;

namespace ClientPagerProto.Controllers
{
    public partial class DemoListController : Controller
    {
        public virtual ActionResult CatList()
        {
            ViewBag.Title = "Multipage list demo";
            ViewBag.VarLeft = "catList1";
            ViewBag.VarRight = "catList2";
            ViewBag.VarLeftOpt = "catList3";
            ViewBag.VarRightOpt = "catList4";

            var p1 = new CategoryListPagedProvider(string.Empty, "p0001678", ViewBag.VarLeft, false);
            var p2 = new CategoryListPagedProvider(string.Empty, "p0001678", ViewBag.VarRight, true);
            var p3 = new CategoryListPagedProvider(string.Empty, "p0001678", ViewBag.VarLeftOpt, false);
            var p4 = new CategoryListPagedProvider(string.Empty, "p0001678", ViewBag.VarRightOpt, true);
            
            var m1 = new PagedListConfigModel("1")
            {
                Pagging = { PageSize = 10, RequestedPageNumber = 1 }                
            };                       
            p1.InitializeListConfig(m1, ControllerContext.HttpContext);

            var m2 = new PagedListConfigModel("2")
            {
                Pagging = { PageSize = 0, RequestedPageNumber = 1 }                
            };            
            p2.InitializeListConfig(m2, ControllerContext.HttpContext);

            var m3 = new PagedListConfigModel("3")
            {
                Pagging = { PageSize = 10, RequestedPageNumber = 1 }
            };
            p3.InitializeListConfig(m3, ControllerContext.HttpContext);

            var m4 = new PagedListConfigModel("4")
            {
                Pagging = { PageSize = 0, RequestedPageNumber = 1 }
            };
            p4.InitializeListConfig(m4, ControllerContext.HttpContext);

            FetchPageHelper(p1, m1, false);
            FetchPageHelper(p2, m2, true);
            FetchPageHelper(p3, m3, false);
            FetchPageHelper(p4, m4, true);

            var tt = m4.Items[22];
            tt.Selected = true;
            m4.Items[22] = tt;

            tt = m2.Items[22];
            tt.Selected = true;
            m2.Items[22] = tt;

            var model = new CatListModel(
                m1, 
                m2,
                m3,
                m4
            );

            return View(model);
        }

        [HttpGet]
        public virtual ActionResult PageData(string variable, int pageNumber, int? size, bool? sortByValue, PagedListSortOrder? sortOrder, bool? fltByValue, 
            PagedListFilteringMode? filteringMode, bool? filterCaseSensitive, string fltVal)
        {
            var provider = new CategoryListPagedProvider(string.Empty, "p0001678", "catList1", variable == "catList2" || variable == "catList4");            

            var m = new PagedListConfigModel()
            {
                Pagging = {PageSize = size ?? 10, RequestedPageNumber = pageNumber}                
            };
            provider.InitializeListConfig(m, ControllerContext.HttpContext);

            
            if (sortOrder.HasValue)
                m.Sorting.SortingOrder = sortOrder.Value;

            if (sortByValue == true)
                m.Sorting.SortByValue = true;

            if (fltByValue == true)
                m.Filtering.FilterByValue = true;

            if (filteringMode.HasValue)
                m.Filtering.FilteringMode = filteringMode.Value;            

            if (filterCaseSensitive.HasValue)
                m.Filtering.FilterCaseSensitive = filterCaseSensitive.Value;

            if (!string.IsNullOrEmpty(fltVal))
                m.Filtering.FilterValue = fltVal.Trim();

            var p = FetchPageHelper(provider, m, variable == "catList2" || variable == "catList4");            
                        
            return Json(JsonHelperNs.ToJson(p, false, true), JsonRequestBehavior.AllowGet);
        }

        DataPage<PagedListItem> FetchPageHelper(CategoryListPagedProvider p, PagedListConfigModel m, bool addAllItem)
        {
            var page = p.FetchPage(m);
            m.MergePage(page, addAllItem);
            return page;
        }
    }
}
