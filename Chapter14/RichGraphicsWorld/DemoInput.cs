using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RichGraphicsWorld
{
    public class DemoInput : IGameplayInput
    {
        struct ScriptedInputState
        {
            public bool MoveForward;
            public bool MoveBackward;
            public bool TurnLeft;
            public bool TurnRight;
        }

        int frameNumber;
        const int maxFrameNumber = 1600;
        Dictionary<int, ScriptedInputState> states = new Dictionary<int, ScriptedInputState>();
        ScriptedInputState currentState;

        public bool MoveForward { get { return currentState.MoveForward; } }

        public bool MoveBackward { get { return currentState.MoveBackward; } }

        public bool TurnLeft { get { return currentState.TurnLeft; } }

        public bool TurnRight { get { return currentState.TurnRight; } }

        public void Start()
        {
            states.Clear();
            states.Add(10, new ScriptedInputState { MoveForward = true });
            states.Add(100, new ScriptedInputState { TurnLeft = true });
            states.Add(160, new ScriptedInputState { TurnRight = true });
            states.Add(280, new ScriptedInputState { TurnLeft = true });
            states.Add(340, new ScriptedInputState { MoveBackward = true });
            states.Add(430, new ScriptedInputState { MoveForward = true });
            states.Add(750, new ScriptedInputState { TurnLeft = true });
            states.Add(840, new ScriptedInputState());
        }

        public void Update()
        {
            frameNumber++;
            if (frameNumber > maxFrameNumber)
                frameNumber = 1;

            ScriptedInputState nextState;
            if (states.TryGetValue(frameNumber, out nextState))
            {
                currentState = nextState;
            }
        }

        public void Draw() { }
        public void Stop() { }
    }
}
