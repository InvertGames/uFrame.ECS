using System;
using uFrame.Attributes;
using uFrame.ECS;
using uFrame.Kernel;
using UniRx;

namespace uFrame.Actions
{
    [ActionTitle("Interval"), uFrameCategory("Timers"), AsyncAction]
    public class Interval : UFAction
    {
        [In]
        public int Minutes;
        [In]
        public int Seconds;

        [In]
        public IEcsComponent DisposeWith;

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
            if (DisposeWith != null)
            {
                Result.DisposeWith(DisposeWith);
            }
        }
    }
    [ActionTitle("Interval By Seconds"), uFrameCategory("Timers"), AsyncAction]
    public class IntervalBySeconds : UFAction
    {

        [In]
        public float Seconds;

        [In]
        public IEcsComponent DisposeWith;

        [Out]
        public Action Tick;

        [Out]
        public IDisposable Result;

        public override void Execute()
        {

            Result = Observable.Interval(TimeSpan.FromSeconds(Seconds)).Subscribe(_ =>
            {
                Tick();
            }).DisposeWith(System);
            if (DisposeWith != null)
            {
                Result.DisposeWith(DisposeWith);
            }
        }
    }
}