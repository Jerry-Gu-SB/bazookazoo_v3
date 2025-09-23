using System;
using Unity.Netcode;
using UnityEngine;

namespace Global_Utils
{
    public class GlobalTick : NetworkBehaviour
    {
        private static GlobalTick Instance { get; set; }
        private float _timer;
        private const int ServerTickRate = 60;  // 60 fps
        public float minTimeBetweenTicks;
        public NetworkVariable<int> CurrentTick { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            minTimeBetweenTicks = 1f / ServerTickRate;
        }

        private void Update()
        {
            if (IsServer)
            {
                _timer += Time.deltaTime;
            }
        }

        public bool ShouldTick()
        {
            if (_timer >= minTimeBetweenTicks)
            {
                _timer -= minTimeBetweenTicks;
                CurrentTick.Value++;
                return true;
            }
            return false;
        }
    }
}
