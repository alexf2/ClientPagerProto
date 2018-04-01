
namespace CqsDataFoundation
{
    public interface IDbContextConfig
    {
        string DefaultConnectionString
        {
            get;
        }

        string GetConnectionString(string name);

        int CommandTimeoutSec
        {
            get;
        }
    }
}
