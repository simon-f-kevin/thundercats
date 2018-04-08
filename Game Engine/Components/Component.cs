using System;

namespace Game_Engine.Components{

    /*
    * Base component class, uses a Guid to garantuee uniqueness when used in a component dictionary-
    */
    public abstract class Component{
        public Guid ComponentId { get; }

        public Component()
        {
            ComponentId = Guid.NewGuid();
        }
    }
}
