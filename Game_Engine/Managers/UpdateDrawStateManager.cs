using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game_Engine.Managers
{
    public class UpdateDrawStateManager
    {
        internal const int NUM_OF_STATES = 3;

        private const int UPDATES_PER_DRAW = -1;

        private static UpdateDrawStateManager theInstance;
        private object x = new object();
        private int next = 1;
        private int current = 0;
        private int draw = 0;
        private int updatesToDo = UPDATES_PER_DRAW;

        /// <summary>
        ///  Manages tripple buffered state for one draw and one update thread.
        /// </summary>
        public static UpdateDrawStateManager Instance
        {
            get {
                if(theInstance == null)
                {
                    theInstance = new UpdateDrawStateManager();
                }
                return theInstance; }
        }
        /// <summary>
        ///  The old/previous state index for updating. This state MUST NOT be written.
        ///  ONLY access from the update thread.
        /// </summary>
        internal int Old
        {
            get { return current; }
        }

        /// <summary>
        ///  The New state index for updating. Updates should be written to this index.
        ///  ONLY access from the update thread.
        /// </summary>
        internal int New
        {
            get { return next; }
        }
        /// <summary>
        ///  The current state index for drawing.
        ///  The Draw thread MUST NOT access the Old or Next state indices.
        /// </summary>
        internal int Draw
        {
            get { return draw; }
        }
        /// <summary>
        ///  Advance the draw index.
        ///  MUST be called from the Draw thread.
        /// </summary>
        public void BeginDrawNewFrame()
        {
            lock (x)
            {
                while (updatesToDo > 0)
                {
                    Monitor.Wait(x);
                }
                draw = current;
                if (updatesToDo == 0)
                {
                    updatesToDo = UPDATES_PER_DRAW;
                    Monitor.PulseAll(x);
                }
            }
        }

        /// <summary>
        ///  Advance the current and next indices.
        ///  MUST be called from the Update thread.
        /// </summary>
        public void EndUpdateNewFrame()
        {
            lock (x)
            {
                while (updatesToDo == 0)
                {
                    Monitor.Wait(x);
                }
                current = next;
                next = (next + 1) % NUM_OF_STATES;
                while (next == current || next == draw)
                {
                    next = (next + 1) % NUM_OF_STATES;
                }
                if(updatesToDo > 0)
                {
                    updatesToDo--;
                }
                if(updatesToDo == 0)
                {
                    Monitor.PulseAll(x);
                }
            }
        }
    }
}
     