using System;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace OMC.Tweening
{
    internal static class TweeningUpdateCore
    {
        private struct OMCUpdate { }
        
        private struct OMCFixeUpdate { }
        
        private struct OMCLateUpdate { }

        private static Action<float> _onOMCUpdateEvent;
        
        public static event Action<float> OnOMCUpdateEvent
        {
            add => _onOMCUpdateEvent += value;
            remove => _onOMCUpdateEvent -= value;
        }

        private static Action<float> _onOMCLateUpdateEvent;
        
        public static event Action<float> OnOMCLateUpdateEvent
        {
            add => _onOMCLateUpdateEvent += value;
            remove => _onOMCLateUpdateEvent -= value;
        }

        private static Action<float> _onOMCFixedUpdateEvent;
        
        public static event Action<float> OnOMCFixedUpdateEvent
        {
            add => _onOMCFixedUpdateEvent += value;
            remove => _onOMCFixedUpdateEvent -= value;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var update = new PlayerLoopSystem
            {
                subSystemList = new []
                {
                    new PlayerLoopSystem
                    {
                        updateDelegate = OnOMCUpdate,
                        type = typeof(OMCUpdate)
                    }
                }
            };

            var fixedUpdate = new PlayerLoopSystem
            {
                subSystemList = new[]
                {
                    new PlayerLoopSystem
                    {
                        updateDelegate = OnOMCFixedUpdate,
                        type = typeof(OMCFixeUpdate)
                    }
                }
            };

            var lateUpdate = new PlayerLoopSystem()
            {
                subSystemList = new[]
                {
                    new PlayerLoopSystem
                    {
                        updateDelegate = OnOMCLateUpdate,
                        type = typeof(OMCLateUpdate)
                    }
                }
            };
            
            var playerLoop = PlayerLoop.GetDefaultPlayerLoop();

            var loopFlag = new []{false, false, false};
            for (int i = 0; i < playerLoop.subSystemList.Length; i++)
            {
                if (playerLoop.subSystemList[i].type == typeof(Update))
                {
                    playerLoop.subSystemList[i] = new()
                    {
                        type = playerLoop.subSystemList[i].type,
                        updateDelegate = playerLoop.subSystemList[i].updateDelegate,
                        subSystemList = playerLoop.subSystemList[i].subSystemList.Append(update).ToArray(),
                        updateFunction = playerLoop.subSystemList[i].updateFunction,
                        loopConditionFunction = playerLoop.subSystemList[i].loopConditionFunction
                    };

                    loopFlag[0] = true;
                }
                else if (playerLoop.subSystemList[i].type == typeof(FixedUpdate))
                {
                    playerLoop.subSystemList[i] = new()
                    {
                        type = playerLoop.subSystemList[i].type,
                        updateDelegate = playerLoop.subSystemList[i].updateDelegate,
                        subSystemList = playerLoop.subSystemList[i].subSystemList.Append(fixedUpdate).ToArray(),
                        updateFunction = playerLoop.subSystemList[i].updateFunction,
                        loopConditionFunction = playerLoop.subSystemList[i].loopConditionFunction
                    };

                    loopFlag[1] = true;
                }
                else if (playerLoop.subSystemList[i].type == typeof(PreLateUpdate))
                {
                    playerLoop.subSystemList[i] = new()
                    {
                        type = playerLoop.subSystemList[i].type,
                        updateDelegate = playerLoop.subSystemList[i].updateDelegate,
                        subSystemList = playerLoop.subSystemList[i].subSystemList.Append(lateUpdate).ToArray(),
                        updateFunction = playerLoop.subSystemList[i].updateFunction,
                        loopConditionFunction = playerLoop.subSystemList[i].loopConditionFunction
                    };

                    loopFlag[2] = true;
                }
                
                if (loopFlag.All(x => x)) break;
            }
            
            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        private static void OnOMCUpdate() => _onOMCUpdateEvent?.Invoke(Time.deltaTime);

        private static void OnOMCFixedUpdate() => _onOMCFixedUpdateEvent?.Invoke(Time.fixedDeltaTime);

        private static void OnOMCLateUpdate() => _onOMCLateUpdateEvent?.Invoke(Time.deltaTime);
    }   
}