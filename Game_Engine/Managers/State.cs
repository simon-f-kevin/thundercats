using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Managers
{
    public class State<T>
    {
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

        public T Current
        {
            get
            {
                return state[UpdateDrawStateManager.Instance.Current];
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

        private T[] state = new T[UpdateDrawStateManager.NUMBER_OF_STATES];
    }
}

