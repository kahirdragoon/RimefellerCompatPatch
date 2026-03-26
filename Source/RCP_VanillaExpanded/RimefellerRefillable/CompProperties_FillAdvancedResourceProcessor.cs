using Rimefeller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RCP_VanillaExpanded
{
    public class CompProperties_FillAdvancedResourceProcessor : CompProperties_Pipe
    {
        public CompProperties_FillAdvancedResourceProcessor() => compClass = typeof(CompFillAdvancedResourceProcessor);
    }
}
