using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RCP_VanillaExpanded
{
    public class RefillableRimefellerPipeSystemCompatManager : MapComponent
    {
        public List<CompRefillFromRimefellerPipeNet> RefillComps { get; } = new List<CompRefillFromRimefellerPipeNet>();

        public RefillableRimefellerPipeSystemCompatManager(Map map) : base(map)
        {

        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            foreach (var comp in RefillComps)
                comp.CompTick2();
        }
    }
}
