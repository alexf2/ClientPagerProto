using System.Collections.Generic;

namespace ClientPagerProto.DataSource.Viking
{
    public sealed class VariableResolver : IVariableResolver
    {
        public const string CategoryList1 = "catList1";
        public const string CategoryList2 = "catList2";

        public const string CategoryList3 = "catList3";
        public const string CategoryList4 = "catList4";

        static readonly SingleVariableMock _var1 = new SingleVariableMock(new List<IVariableCategory>
            {                
                new VariableCategory(){Code="2AA00", Text="Corporate Groups (2AA00)"},
                new VariableCategory(){Code="2AA01", Text="International Businesses (2AA01)"},
                new VariableCategory(){Code="2AA'02", Text="US Businesses (2AA'02)"},
                new VariableCategory(){Code="3AA''02", Text="Corp HR  CSR (3AA''02)"},

                new VariableCategory(){Code="3AA04", Text="Financial Management (3AA04)"},
                new VariableCategory(){Code="3AA05", Text="Global Bus  Tech Solutions (3AA05)"},
                new VariableCategory(){Code="3AA07", Text="Global Mktg Communications (3AA07)"},
                new VariableCategory(){Code="3AA20", Text="LCBE (3AA20)"},
                new VariableCategory(){Code="3AA18", Text="The Executive Office (3AA18)"},

                new VariableCategory(){Code="3AA06", Text="Global Commodities (3AA06)"},
                new VariableCategory(){Code="3AA10", Text="International Executives (3AA10)"},
                new VariableCategory(){Code="3AA11", Text="International Insurance (3AA11)"},
                new VariableCategory(){Code="3AA00", Text="Agency Distribution (3AA00)"},
                new VariableCategory(){Code="3AA01", Text="Annuities (3AA01)"},

                new VariableCategory(){Code="3AA22", Text="DOM CENTRALIZED SUPP FUNCTIONS (3AA22)"},
                new VariableCategory(){Code="3AA08", Text="Group Insurance (3AA08)"},
                new VariableCategory(){Code="3AA09", Text="Individual Life (3AA09)"},
                new VariableCategory(){Code="3AA14", Text="Pru Real Estate  Relo Svcs (3AA14)"},
                new VariableCategory(){Code="3AA21", Text="Prudential Asset Management (3AA21)"},

                new VariableCategory(){Code="3AA17", Text="Prudential Retirement (3AA17)"},
                new VariableCategory(){Code="3AA19", Text="USB/Center (3AA19)"},
                new VariableCategory(){Code="4AA00", Text="AD-Business Quality (4AA00)"},
                new VariableCategory(){Code="4AA01", Text="AD-Comp  Planning Exec (4AA01)"},
                new VariableCategory(){Code="4AA02", Text="AD-CompensationPlanning (4AA02)"},

                new VariableCategory(){Code="4AA03", Text="AD-Eastern Territory Exec (4AA03)"},
                new VariableCategory(){Code="4AA04", Text="AD-Executive (4AA04)"},
                new VariableCategory(){Code="4AA05", Text="AD-Executive Support (4AA0)"},
                new VariableCategory(){Code="4AA06", Text="AD-Field (4AA06)"},
                new VariableCategory(){Code="4AA07", Text="AD-Field (4AA07)"},

                new VariableCategory(){Code="4AA08", Text="AD-Field (4AA08)"},
                new VariableCategory(){Code="4AA09", Text="AD-Field (4AA09)"},

                new VariableCategory(){Code="4AA091", Text="AD-Field (4AA091)"},
                new VariableCategory(){Code="4AA092", Text="AD-Field (4AA092)"},
                new VariableCategory(){Code="4AA093", Text="AD-Field (4AA093)"},
                new VariableCategory(){Code="4AA094", Text="AD-Field (4AA094)"},
                new VariableCategory(){Code="4AA095", Text="AD-Field (4AA095)"},
                new VariableCategory(){Code="4AA096", Text="AD-Field (4AA096)"},
                new VariableCategory(){Code="4AA097", Text="AD-Field (4AA097)"},
                new VariableCategory(){Code="4AA098", Text="AD-Field (4AA098)"}
            }, false
        );

        static readonly SingleVariableMock _var2 = new SingleVariableMock(new List<IVariableCategory>
            {
                new VariableCategory(){Code="Category1", Text="Category1"},
                new VariableCategory(){Code="Mixed text in русский and English", Text="Mixed text in русский and English"},
                new VariableCategory(){Code="This is open text", Text="This is open text"},
                new VariableCategory(){Code="text contains special symbols !@#$%^&*()_+<>{}[]'\"\\/!~,;:-+=<>\";", Text="text contains special symbols !@#$%^&*()_+<>{}[]'\"\\/!~,;:-+=<>\";"},
                new VariableCategory(){Code="大岛的弟地东都对多", Text="大岛的弟地东都对多"},
                new VariableCategory(){Code="نا عربي واريد ان اتعلم اللغه الروسيه", Text="نا عربي واريد ان اتعلم اللغه الروسيه"},                

                new VariableCategory(){Code="2AA00", Text="Corporate Groups (2AA00)"},
                new VariableCategory(){Code="2AA01", Text="International Businesses (2AA01)"},
                new VariableCategory(){Code="2AA'02", Text="US Businesses (2AA'02)"},
                new VariableCategory(){Code="3AA''02", Text="Corp HR  CSR (3AA''02)"},

                new VariableCategory(){Code="3AA04", Text="Financial Management (3AA04)"},
                new VariableCategory(){Code="3AA05", Text="Global Bus  Tech Solutions (3AA05)"},
                new VariableCategory(){Code="3AA07", Text="Global Mktg Communications (3AA07)"},
                new VariableCategory(){Code="3AA20", Text="LCBE (3AA20)"},
                new VariableCategory(){Code="3AA18", Text="The Executive Office (3AA18)"},

                new VariableCategory(){Code="3AA06", Text="Global Commodities (3AA06)"},
                new VariableCategory(){Code="3AA10", Text="International Executives (3AA10)"},
                new VariableCategory(){Code="3AA11", Text="International Insurance (3AA11)"},
                new VariableCategory(){Code="3AA00", Text="Agency Distribution (3AA00)"},
                new VariableCategory(){Code="3AA01", Text="Annuities (3AA01)"},

                new VariableCategory(){Code="3AA22", Text="DOM CENTRALIZED SUPP FUNCTIONS (3AA22)"},
                new VariableCategory(){Code="3AA08", Text="Group Insurance (3AA08)"},
                new VariableCategory(){Code="3AA09", Text="Individual Life (3AA09)"},
                new VariableCategory(){Code="3AA14", Text="Pru Real Estate  Relo Svcs (3AA14)"},
                new VariableCategory(){Code="3AA21", Text="Prudential Asset Management (3AA21)"},

                new VariableCategory(){Code="3AA17", Text="Prudential Retirement (3AA17)"},
                new VariableCategory(){Code="3AA19", Text="USB/Center (3AA19)"},
                new VariableCategory(){Code="4AA00", Text="AD-Business Quality (4AA00)"},
                new VariableCategory(){Code="4AA01", Text="AD-Comp  Planning Exec (4AA01)"},
                new VariableCategory(){Code="4AA02", Text="AD-CompensationPlanning (4AA02)"},

                new VariableCategory(){Code="4AA03", Text="AD-Eastern Territory Exec (4AA03)"},
                new VariableCategory(){Code="4AA04", Text="AD-Executive (4AA04)"},
                new VariableCategory(){Code="4AA05", Text="AD-Executive Support (4AA05)"},
                new VariableCategory(){Code="4AA06", Text="AD-Field (4AA06)"},
                new VariableCategory(){Code="4AA07", Text="AD-Field (4AA07)"},

                new VariableCategory(){Code="4AA08", Text="AD-Field (4AA08)"},
                new VariableCategory(){Code="4AA09", Text="AD-Field (4AA09)"},

                new VariableCategory(){Code="4AA091", Text="AD-Field (4AA091)"},
                new VariableCategory(){Code="4AA092", Text="AD-Field (4AA092)"},
                new VariableCategory(){Code="4AA093", Text="AD-Field (4AA093)"},
                new VariableCategory(){Code="4AA094", Text="AD-Field (4AA094)"},
                new VariableCategory(){Code="4AA095", Text="AD-Field (4AA095)"},
                new VariableCategory(){Code="4AA096", Text="AD-Field (4AA096)"},
                new VariableCategory(){Code="4AA097", Text="AD-Field (4AA097)"},
                new VariableCategory(){Code="4AA098", Text="AD-Field (4AA098)"}
            }, true
        );

        public IVariable GetVariable(string loopId, string projectCode, string varId)
        {
            switch (varId)
            {
                case CategoryList1: case CategoryList3:
                    return _var1;

                default:
                    return _var2;
            }
        }

    }
}
