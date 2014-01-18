using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColiSys
{
    public enum Bounds { l, u };
    public enum ENode { adj, dwn };
    public enum copyTypes {	copyNode,copyDwn,copyAdj,copyBoth};
    public enum OverlapType { Right, Left, OEA, AEO, Equals, Before, After };
    public enum Shape{Circle, Square};

}
