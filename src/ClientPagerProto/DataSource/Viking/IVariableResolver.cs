
namespace ClientPagerProto.DataSource.Viking
{
    public interface IVariableResolver
    {
        IVariable GetVariable(string loopId, string projectCode, string selectedId);
    }
}
