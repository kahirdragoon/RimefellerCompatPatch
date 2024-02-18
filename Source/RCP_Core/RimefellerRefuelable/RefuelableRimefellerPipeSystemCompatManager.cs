using Rimefeller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RCP_Core
{
    public class RefuelableRimefellerPipeSystemCompatManager : MapComponent
    {
        public List<CompRefuelFromRimefellerPipeNet> RefuelComps { get; } = new List<CompRefuelFromRimefellerPipeNet>();

        public RefuelableRimefellerPipeSystemCompatManager(Map map) : base(map)
        {
            
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            foreach(var comp in RefuelComps)
                comp.CompTick2();
        }
    }
}
