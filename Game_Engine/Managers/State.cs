using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Managers
{
    public class State<T>
    {
        // New is the modified Value after an update has been updated
        public T New
        {
            get
            {
                return state[UpdateDrawStateManager.Instance.New];
            }
            set
            {
                state[UpdateDrawStateManager.Instance.New] = value;
            }
        }

        // Old is the current Value before an update has occured 
        public T Old
        {
            get
            {
                return state[UpdateDrawStateManager.Instance.Old];
            }
        }

        public T Draw
        {
            get
            {
                return state[UpdateDrawStateManager.Instance.Draw];
            }
        }

        public State() { }

        public State(T value)
        {
            for (int i = 0; i < state.Length; i++)
            {
                state[i] = value;
            }
        }

        private T[] state = new T[UpdateDrawStateManager.NUM_OF_STATES];
    }
}
