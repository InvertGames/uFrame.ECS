using System;
using uFrame.Attributes;
using uFrame.Kernel;
using UniRx;

namespace uFrame.Actions
{
    [ActionTitle("Interval"),uFrameCategory("Timers")]
    public class Interval : UFAction
    {
        [In]
        public int Minutes;
        [In]
        public int Seconds;
        [Out]
        public Action Tick;
        [Out]
        public IDisposable Result;

        public override void Execute()
        {
            Result = Observable.Interval(new TimeSpan(0, 0, Minutes, Seconds, 0)).Subscribe(_ =>
            {
                Tick();
            }).DisposeWith(System);
        }
    }
}