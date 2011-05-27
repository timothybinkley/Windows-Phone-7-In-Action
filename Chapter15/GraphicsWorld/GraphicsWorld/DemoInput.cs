using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace GraphicsWorld
{
    public class DemoInput : IGamePlayInput
    {
        struct ScriptedInputState
        {
            public bool MoveForward;
            public bool MoveBackward;
            public bool TurnLeft;
            public bool TurnRight;
        }

        ScriptedInputState currentState;
        Dictionary<int, ScriptedInputState> states;
        int frameNumber;

        public DemoInput()
        {
            states = new Dictionary<int, ScriptedInputState>();
            states.Add(10, new ScriptedInputState { MoveForward = true });
            states.Add(100, new ScriptedInputState { TurnLeft = true });
            states.Add(160, new ScriptedInputState { TurnRight = true });
            states.Add(280, new ScriptedInputState { TurnLeft = true });
            states.Add(340, new ScriptedInputState { MoveBackward = true });
            states.Add(430, new ScriptedInputState { MoveForward = true });
            states.Add(750, new ScriptedInputState { TurnLeft = true });
            states.Add(840, new ScriptedInputState());
        }

        public bool MoveForward { get { return currentState.MoveForward; } }
        public bool MoveBackward { get { return currentState.MoveBackward; } }
        public bool TurnLeft { get { return currentState.TurnLeft; } }
        public bool TurnRight { get { return currentState.TurnRight; } }

        public void Initialize(ContentManager content) { }
        public void Start() { }

        public void Update()
        {
            frameNumber++;

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
