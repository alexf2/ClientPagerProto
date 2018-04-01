using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ClientPagerProto.DataSource.Viking;
using ClientPagerProto.PaggedList;
using CqsDataFoundation.Query;
using CqsDataFoundation.Query.Decorators;

namespace ClientPagerProto.DataSource
{
    public sealed class CategoryListPagedProvider: PagedListDataProviderBase<VariableCategoriesPagedQuery, IVariableCategory, DataPage<IVariableCategory>>
    {
        private string _loopId, _projectCode, _varId;
        private bool _addStaticItems;

        public CategoryListPagedProvider(string loopId, string projectCode, string varId, bool addStaticItems)
        {        
            _loopId = loopId;
            _projectCode = projectCode;
            _varId = varId;
            _addStaticItems = addStaticItems;
        }

        protected override string ValueFieldName { get { return "Code"; } }
        protected override string DescriptionFieldName { get { return "Text"; } }

        protected override PagedListItem ListItemFromDataItem(IVariableCategory data)
        {
            return new PagedListItem() {Value = data.Code, Description = data.Text};
        }

        private static readonly List<PagedListItem> _staticItems = new List<PagedListItem>() { new PagedListItem() { Value = PagedListConfigModel.AllValue, Description = PagedListConfigModel.AllText} };

        protected override IList<PagedListItem> GetStaticItems(PagedListConfigModel m, VariableCategoriesPagedQuery q)
        {
            return _addStaticItems && q.PageNumber == 1 ? _staticItems : null;
        }

        protected override DataPage<IVariableCategory> QueryData(VariableCategoriesPagedQuery q)
        {            
            IVariable var = (new VariableResolver()).GetVariable(_loopId, _projectCode, _varId);

            using (var hndl = new ValidationQueryHandlerDecorator<VariableCategoriesPagedQuery, DataPage<IVariableCategory>>(null,
                                new VariableCategoriesQueryHandler(
                                    new VariableDataContext(var))))
                                    

                return hndl.Handle(q);
        }

        public void InitializeListConfig(PagedListConfigModel m, HttpContextBase ctx)
        {
            // {0}/{1}/{2}/{{var}}/{{page}}
            m.DataServiceUrl = string.Format("{2}/{{var}}/{{page}}", UrlHelper.GenerateContentUrl("~/", ctx), MVC.DemoList.Name, MVC.DemoList.ActionNames.PageData);

            /*var urlHlp = new UrlHelper(ctx.Request.RequestContext);
            m.TemplateUrl = urlHlp.Content("~/Content/pagged-list-template.html");*/
        }        
    }
}
