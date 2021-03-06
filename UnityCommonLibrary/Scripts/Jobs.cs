﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
    public class Jobs : MonoSingleton<Jobs>
    {
        private List<Func<bool>> onUpdateJobs = new List<Func<bool>>();
        private List<Func<bool>> onFixedUpdateJobs = new List<Func<bool>>();
        private List<Func<bool>> onLateUpdateJobs = new List<Func<bool>>();

        public static void ExecuteOnUpdate(Func<bool> func)
        {
            get.onUpdateJobs.Add(func);
        }
        public static void ExecuteOnFixedUpdate(Func<bool> func)
        {
            get.onFixedUpdateJobs.Add(func);
        }
        public static void ExecuteOnLateUpdate(Func<bool> func)
        {
            get.onLateUpdateJobs.Add(func);
        }
        public static void ExecuteCoroutine(IEnumerator routine)
        {
            get.StartCoroutine(routine);
        }
        public static void ExecuteDelayed(float delay, Action action)
        {
            get.StartCoroutine(get._ExecuteDelayed(delay, action));
        }
        public static void ExecuteNextFrame(Action action)
        {
            get.StartCoroutine(get._ExecuteNextFrame(action));
        }

        private IEnumerator _ExecuteDelayed(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }
        private IEnumerator _ExecuteNextFrame(Action action)
        {
            yield return null;
            action();
        }
        private void Update()
        {
            for (int i = onUpdateJobs.Count - 1; i >= 0; i--)
            {
                var job = onUpdateJobs[i];
                if (job == null || !job())
                {
                    onUpdateJobs.RemoveAt(i);
                }
            }
        }
        private void FixedUpdate()
        {
            for (int i = onFixedUpdateJobs.Count - 1; i >= 0; i--)
            {
                var job = onFixedUpdateJobs[i];
                if (job == null || !job())
                {
                    onFixedUpdateJobs.RemoveAt(i);
                }
            }
        }
        private void LateUpdate()
        {
            for (int i = onLateUpdateJobs.Count - 1; i >= 0; i--)
            {
                var job = onLateUpdateJobs[i];
                if (job == null || !job())
                {
                    onLateUpdateJobs.RemoveAt(i);
                }
            }
        }
    }
}