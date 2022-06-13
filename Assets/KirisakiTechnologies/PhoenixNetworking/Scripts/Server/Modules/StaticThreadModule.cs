using System;
using System.Collections.Generic;
using KirisakiTechnologies.GameSystem.Scripts.Modules;
using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.Server.Modules
{
    public class StaticThreadModule : GameModuleBaseMono, IThreadModule
    {
        #region Public

        public static void ExecuteOnMainThread(Action action)
        {
            if (action == null)
            {
                Debug.Log("No action to execute on main thread!");
                return;
            }

            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(action);
                actionToExecuteOnMainThread = true;
            }
        }

        #endregion

        #region Private

        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;

        private static void UpdateMain()
        {
            if (!actionToExecuteOnMainThread)
                return;

            executeCopiedOnMainThread.Clear();
            lock (executeOnMainThread)
            {
                executeCopiedOnMainThread.AddRange(executeOnMainThread);
                executeOnMainThread.Clear();
                actionToExecuteOnMainThread = false;
            }

            foreach (var action in executeCopiedOnMainThread)
                action();
        }

        #endregion

        #region MonoBehaviour Methods

        private void Update()
        {
            UpdateMain();
        }

        #endregion
    }
}