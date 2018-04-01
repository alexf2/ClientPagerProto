using ClientPagerProto.PaggedList;

namespace ClientPagerProto.Models
{
    public class CatListModel
    {
        public CatListModel(IPagedListConfigModel left, IPagedListConfigModel right, IPagedListConfigModel leftOpt, IPagedListConfigModel rightOpt)
        {
            LeftListModel = left;
            RightListModel = right;

            LeftListModelOption = leftOpt;
            RightListModelOption = rightOpt;
        }

        public IPagedListConfigModel LeftListModel { get; private set; }
        public IPagedListConfigModel RightListModel { get; private set; }

        public IPagedListConfigModel LeftListModelOption { get; private set; }
        public IPagedListConfigModel RightListModelOption { get; private set; }
    }
}