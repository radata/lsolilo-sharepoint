using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPS.Core.QueryBuilder.Enums
{
    public enum CamlQuerySchemaElements
    {
        [CamlQuerySchemaElementsAttribute("<Eq><FieldRef %FieldAttributes% /><Value %ValueAttribute% >%Value%</Value></Eq>")]
        Eq,
        [CamlQuerySchemaElementsAttribute("<Neq><FieldRef %FieldAttributes% /><Value %ValueAttribute% >%Value%</Value></Neq>")]
        Neq,
        [CamlQuerySchemaElementsAttribute("<Geq><FieldRef %FieldAttributes% /><Value %ValueAttribute% >%Value%</Value></Geq>")]
        Geq,
        [CamlQuerySchemaElementsAttribute("<Contains><FieldRef %FieldAttributes% /><Value %ValueAttribute% >%Value%</Value></Contains>")]
        Contains,
        [CamlQuerySchemaElementsAttribute("<Lt><FieldRef %FieldAttributes% /><Value %ValueAttribute% >%Value%</Value></Lt>")]
        Lt,
        [CamlQuerySchemaElementsAttribute("<Leq><FieldRef %FieldAttributes% /><Value %ValueAttribute% >%Value%</Value></Leq>")]
        Leq,
        [CamlQuerySchemaElementsAttribute("<Or>%Value%</Or>")]
        Or,
        [CamlQuerySchemaElementsAttribute("<And>%Value%</And>")]
        And,
        [CamlQuerySchemaElementsAttribute("<Where>%Value%</Where>")]
        Where,
        [CamlQuerySchemaElementsAttribute("<Query>%Value%</Query>")]
        Query,
        [CamlQuerySchemaElementsAttribute("<IsNull><FieldRef %FieldAttributes% ></FieldRef></IsNull>")]
        IsNull,
        [CamlQuerySchemaElementsAttribute("<IsNotNull><FieldRef %FieldAttributes% ></FieldRef></IsNotNull>")]
        IsNotNull
    }
}
