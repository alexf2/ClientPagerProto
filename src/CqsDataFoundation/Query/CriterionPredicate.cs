
namespace CqsDataFoundation.Query
{
    public enum CriterionPredicate
    {
        Eq,     //==
        Neq,    //!=
        Lt,     //>
        Gt,     //<
        Null,   //== null
        NotNull, //!= null
        LtEq,   //<=
        GtEq,   //>=

        Contains,
        StartsWith,
        EndsWith
    };
}
