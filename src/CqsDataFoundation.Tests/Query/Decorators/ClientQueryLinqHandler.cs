using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation.Query;

namespace CqsDataFoundation.Tests.Query.Decorators
{
    class ClientQueryLinqHandler : QueryHandlerBase<ClientDataContext, ClientQuery, Client>
    {
        public ClientQueryLinqHandler(ClientDataContext context, bool sharedContext = false)
            : base(context, sharedContext)
        {
        }

        public override Client Handle(ClientQuery q)
        {
            return new Client()
            {
                GivenName = q.GivenName,
                SurName = q.SurName,

                Referer = new Referer() { Code = "12786" }
            };
        }
    }
}
