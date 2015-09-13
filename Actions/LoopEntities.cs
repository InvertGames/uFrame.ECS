using System;
using System.Collections;
using System.Collections.Generic;
using uFrame.Attributes;
using uFrame.ECS;

namespace uFrame.Actions
{
    [ActionTitle("Loop Entities"), uFrameCategory("Loops", "Entities")]
    public class LoopEntities<TType> : UFAction where TType : class, IEcsComponent
    {
        [Out]
        public Action Continue;
        
        [Out]
        public TType Item;

        public override void Execute()
        {
            var items = System.ComponentSystem.RegisterComponent<TType>().Components;
            foreach (var item in items)
            {
                Item = (TType)item;
                if (Continue != null)
                    Continue();
            }
        }
    }
}