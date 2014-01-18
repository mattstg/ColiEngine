using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColiSys
{
    class NodeSpider
    {
        public Node prev = null;
        public Node cur  = null;
        public Node next = null;

        public NodeSpider(){}
        public NodeSpider(Node start) { SetCur(start); }    

        public void It() //iterates making sure doesnt move
        {
            
            if (next != null)
            {
                 prev = cur;
                 cur = next;
                 next = cur.Adj();
            }
        }

        public void SetCur(Node newCur)
        {
            cur = newCur; next = cur.Adj(); prev = null;
        }


    }
}
