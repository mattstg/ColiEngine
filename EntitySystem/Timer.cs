using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Global
{
    public class Timers
    {
        bool pause = false;
        float minT;
        float targetT;
        float maxT;   //maxes out at max float


        private float _curT;
        public float curT { get { return _curT; } set { _curT = value; _curT = (_curT > maxT) ? maxT : (_curT < minT) ? minT : _curT; } }//if (curT > maxT) curT = maxT; } }//limits curT to maxT
        //paired vars ^

        public bool ready { get { return (curT > targetT); } } //return if ready
        public float timesOver { get { return (targetT > 0) ? curT / targetT : 0; } }  //return how many times is crosses over, 0 is targetT is 0

        public Timers() { minT = 0; maxT = 0; curT = 0; targetT = 0; }
        public Timers(float min, float target, float max, float cur) { minT = min; maxT = max; curT = cur; targetT = target; if (curT > maxT) curT = maxT; }
        public Timers(float min, float target, float max) { minT = min; maxT = max; curT = 0; targetT = target; if (curT > maxT) curT = maxT; }
        public Timers(float target, float max) { minT = 0; maxT = max; curT = 0; targetT = target; if (curT > maxT) curT = maxT; }
        public Timers(float target) { minT = 0; maxT = float.MaxValue; curT = 0; targetT = target; if (curT > maxT) curT = maxT; }

        public void Tick(float timePassed)
        {
            if (!pause)
                curT += timePassed;

        }

        public void Dec(bool startOver) //start over or decerment by the target time
        {
            curT -= (startOver) ? curT : targetT;

        }

        public void Pause(bool pause)
        {
            this.pause = pause;

        }

        public Timers Copy()
        {
            return new Timers(minT, targetT, maxT, curT);
        }


    }

    
}
