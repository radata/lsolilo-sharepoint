using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPS.Core.QueryBuilder.Enums
{
    public enum CamlQuerySchemaAttributes
    {
        [CamlQuerySchemaElementsAttribute("Type='%Value%'")]
        Type,
        [CamlQuerySchemaElementsAttribute("Name='%Value%'")]
        Name,
        [CamlQuerySchemaElementsAttribute("LookupId='%Value%'")]
        LookupId,
    }
}
