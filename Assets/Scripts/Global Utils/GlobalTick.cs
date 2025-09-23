using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Global_Utils
{
    public class GlobalTick : NetworkBehaviour
    {
        public static GlobalTick Instance { get; private set; }
        public float minTimeBetweenTicks;
        public UnityEvent tickEvent;
        
        private float _timer;
        private const int ServerTickRate = 60;  // 60 fps
        private NetworkVariable<int> CurrentTick { get; set; }
        

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            minTimeBetweenTicks = 1f / ServerTickRate;
            CurrentTick = new NetworkVariable<int>(0);
            tickEvent = new UnityEvent();
        }
        private void FixedUpdate()
        {
            if (!IsServer) return;
            _timer += Time.deltaTime;
            if (_timer >= minTimeBetweenTicks)
            {
                _timer -= minTimeBetweenTicks;
                CurrentTick.Value++;
                tickEvent.Invoke();
            }
        }
        public int GetCurrentTick()
        {
            return CurrentTick.Value;
        }
    }
}
