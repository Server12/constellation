using System.Collections.Generic;
using Constellation.Module_1.Logic;

namespace Constellation.Module_1.Data
{
    public class StarNode
    {
        public readonly Star StarView;
        
        public HashSet<StarNode> Neighbours { get; } = new HashSet<StarNode>();

        public StarNode(Star starView)
        {
            StarView = starView;
        }
    }
}