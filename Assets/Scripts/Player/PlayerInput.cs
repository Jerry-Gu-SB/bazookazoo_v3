using Global_Utils;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerInput : NetworkBehaviour
    {
        public struct InputPayLoad : INetworkSerializable
        {
            public int Tick;
            public Vector2 InputVector;
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref Tick);
                serializer.SerializeValue(ref InputVector);
            }
        }
        public InputPayLoad GetInput()
        {
            InputPayLoad input = new InputPayLoad
            {
                Tick = GlobalTick.Instance.GetCurrentTick(),
                InputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))
            };
            return input;
        }
    }
}
