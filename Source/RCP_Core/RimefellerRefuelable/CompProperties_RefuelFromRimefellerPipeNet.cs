using Rimefeller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RCP_Core
{
    public class CompProperties_RefuelFromRimefellerPipeNet : CompProperties_Pipe
    {
        public CompProperties_RefuelFromRimefellerPipeNet() => compClass = typeof(CompRefuelFromRimefellerPipeNet);
    }
}
