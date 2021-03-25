using System;
using System.Collections;
using System.Collections.Generic;
using LpmGames.Utils.Debug;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LpmGames.Utils.ControlFlow
{
    public static class CoroutineDirector
    {
        private static MonoBehaviour Executor
        {
            get
            {
                if (_executor != null) return _executor;
                var executorContainer = new GameObject("CoroutineDirector");
                Object.DontDestroyOnLoad(executorContainer);
                _executor = executorContainer.AddComponent<CoroutineDirectorBehaviour>();
                return _executor;
            }
        }

        private static MonoBehaviour _executor;

        private static CoroutinePlus StartCoroutine(Yielder yielder)
        {
            var routine = new CoroutinePlus(yielder);
            StartCoroutine(routine);
            return routine;
        }
        private static PipeableCoroutine<T> StartPipeableCoroutine<T>(WaitForResult<T> waiter)
        {
            var routine = new PipeableCoroutine<T>(waiter);
            StartCoroutine(routine);
            return routine;
        }
        
        private static CoroutinePlus CreateCoroutine(Yielder yielder)
        {
            var routine = new CoroutinePlus(yielder);
            return routine;
        }
        
        public static CoroutinePlus StartCoroutine(YieldInstruction yield) => StartCoroutine(new Yielder(yield));
        public static CoroutinePlus StartCoroutine(IEnumerator enumerator) => StartCoroutine(new Yielder(enumerator));
        public static CoroutinePlus StartCoroutine(Func<IEnumerator> enumerator) => StartCoroutine(new Yielder(enumerator));
        public static CoroutinePlus StartCoroutine(Action action) => StartCoroutine(new Yielder(action));
        public static CoroutinePlus CreateCoroutine(YieldInstruction yield) => CreateCoroutine(new Yielder(yield));
        public static CoroutinePlus CreateCoroutine(IEnumerator enumerator) => CreateCoroutine(new Yielder(enumerator));
        public static CoroutinePlus CreateCoroutine(Func<IEnumerator> enumerator) => CreateCoroutine(new Yielder(enumerator));
        public static CoroutinePlus CreateCoroutine(Action action) => CreateCoroutine(new Yielder(action));
        
        // The WaitForResult<T> construct gives us the ability to clean up syntax by piping results from Then()
        public static PipeableCoroutine<T> StartCoroutine<T>(WaitForResult<T> waiter) => StartPipeableCoroutine(waiter);

        public static void StartCoroutine(CoroutinePlus coroutine)
        {
            coroutine.SetCoroutine(Executor.StartCoroutine(coroutine.Run()));
        }

        private static void StopCoroutine(Coroutine coroutine)
        {
            Executor.StopCoroutine(coroutine);
        }
        
        private class CoroutineDirectorBehaviour : MonoBehaviour{}

        public class CoroutineGroup
        {
            private readonly List<Yielder> _yielders = new List<Yielder>();
            private CoroutinePlus[] _coroutines;
            
            public float Progress { get; private set; }

            public delegate void OnProgress(float progress);
            public event OnProgress ProgressChanged;

            public CoroutineGroup With(params YieldInstruction[] yield)
            {
                foreach(var y in yield) _yielders.Add(new Yielder(y));
                return this;
            }

            public CoroutineGroup With(params IEnumerator[] enumerator)
            {
                foreach(var y in enumerator) _yielders.Add(new Yielder(y));
                return this;
            }

            public CoroutineGroup With(params Func<IEnumerator>[] enumerator)
            {
                foreach(var y in enumerator) _yielders.Add(new Yielder(y));
                return this;
            }

            public CoroutineGroup With(params Action[] action)
            {
                foreach(var y in action) _yielders.Add(new Yielder(y));
                return this;
            }

            private void StartAll()
            {
                _coroutines = new CoroutinePlus[_yielders.Count];
                for (var i = 0; i < _yielders.Count; i++)
                {
                    _coroutines[i] = StartCoroutine(_yielders[i]);
                }
            }

            public CoroutinePlus All(int maxConcurrent)
            {
                // Rolling start..
                _coroutines = new CoroutinePlus[_yielders.Count];
                return StartCoroutine(AwaitAllCapped(maxConcurrent));
            }
            
            public CoroutinePlus All()
            {
                StartAll();
                return StartCoroutine(AwaitAll());
            }
            
            public CoroutinePlus First()
            {
                StartAll();
                return StartCoroutine(AwaitOne());
            }

            private IEnumerator AwaitAllCapped(int maxConcurrent)
            {
                var total = (float)_coroutines.Length;
                var currentMax = 0;
                while (true)
                {
                    var complete = 0;
                    for (var index = 0; index < currentMax && index < _coroutines.Length; index++)
                    {
                        var coroutine = _coroutines[index];
                        if (!coroutine.Running) complete++;
                    }

                    var newSlotsAvailable = maxConcurrent - (currentMax - complete);
                    for(var i = 0; i < newSlotsAvailable; i++)
                    {
                        var index = currentMax + i;
                        if (index >= _coroutines.Length) break;
                        _coroutines[index] = StartCoroutine(_yielders[index]);
                    }
                    currentMax += newSlotsAvailable;

                    Progress = complete / total;
                    ProgressChanged?.Invoke(Progress);
                    if (complete >= total) break;
                    yield return null;
                }
                
                Log.Message("Completed awaiting all coroutines");
            }
            
            private IEnumerator AwaitAll()
            {
                var total = (float)_coroutines.Length;
                while (true)
                {
                    var complete = 0f;
                    foreach (var coroutine in _coroutines)
                    {
                        if (!coroutine.Running) complete++;
                    }
                    Progress = complete / total;
                    ProgressChanged?.Invoke(Progress);
                    if (complete >= total) break;
                    yield return null;
                }
                
                Log.Message("Completed awaiting all coroutines");
            }
            
            private IEnumerator AwaitOne()
            {
                while (true)
                {
                    foreach (var coroutine in _coroutines)
                    {
                        if (coroutine.Running) continue;
                        yield break;
                    }
                    yield return null;
                }
            }
        }
        
        internal class Yielder
        {
            private IEnumerator _enumerator;
            private Func<IEnumerator> _enumeratorGetter;
            
            private static IEnumerator DeferYield(YieldInstruction next)
            {
                yield return next;
            }
            
            private static IEnumerator DeferAction(Action next)
            {
                yield return null;
                next();
            }

            public Yielder(Action next)
            {
                _enumerator = DeferAction(next);
            }
            
            public Yielder(YieldInstruction instruction)
            {
                _enumerator = DeferYield(instruction);
            }

            public Yielder(IEnumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public Yielder(Func<IEnumerator> method)
            {
                _enumeratorGetter = method;
            }

            public IEnumerator Enumerator
            {
                get
                {
                    if (_enumeratorGetter != null) return _enumeratorGetter();
                    return _enumerator;
                }
            }
        }

        public class PipeableCoroutine<T> : CoroutinePlus
        {
            private readonly WaitForResult<T> _waiter;

            public PipeableCoroutine(WaitForResult<T> waiter) : base(new Yielder(waiter))
            {
                _waiter = waiter;
            }
            
            public CoroutinePlus Then(Action<T> next) => Then(new Yielder(() =>
            {
                if (_waiter == null) return;
                next(_waiter.Result);
            }));
            public T Result => _waiter.Result;
        }

        public class CoroutinePlus
        {
            public bool Running { get; private set; }
            public float StartTime { get; private set; }
            public float EndTime { get; private set; }
            public bool WasStopped { get; private set; }

            private Queue<CoroutinePlus> _then;

            private WaitForContinue _awaitContinue;
            private bool _hasRunAndFinished;
            
            public float Runtime
            {
                get
                {
                    if (Running) return Time.time - StartTime;
                    return EndTime - StartTime;
                }
            }

            internal CoroutinePlus Then(Yielder next)
            {
                if (_hasRunAndFinished)
                    return StartCoroutine(next);
                
                if(_then == null) _then = new Queue<CoroutinePlus>();
                var promised = new CoroutinePlus(next);
                _then.Enqueue(promised);
                return promised;
            }
            
            public CoroutinePlus Then(YieldInstruction next) => Then(new Yielder(next));
            public CoroutinePlus Then(IEnumerator next) => Then(new Yielder(next));
            public CoroutinePlus Then(Func<IEnumerator> next) => Then(new Yielder(next));
            public CoroutinePlus Then(Action next) => Then(new Yielder(next));
            
            private Coroutine _coroutine;
            private readonly Yielder _original;

            public IEnumerator AwaitFinish(float timeout = 0)
            {
                if (_hasRunAndFinished) yield break;
                
                if(_awaitContinue == null) _awaitContinue = new WaitForContinue(timeout);
                yield return _awaitContinue;
            }
            
            public void SetCoroutine(Coroutine coroutine)
            {
                _coroutine = coroutine;
            }

            public void Stop()
            {
                if (!Running) return;
                WasStopped = true;
                Running = false;
                EndTime = Time.time;
                StopCoroutine(_coroutine);
                _awaitContinue?.Continue();
            }
            
            internal CoroutinePlus(Yielder original)
            {
                _original = original;
            }
            
            public IEnumerator Run()
            {
                Running = true;
                StartTime = Time.time;
                yield return _original.Enumerator;
                EndTime = Time.time;
                Running = false;
                _awaitContinue?.Continue();
                _hasRunAndFinished = true;
                while (_then != null && _then.Count > 0)
                {
                    StartCoroutine(_then.Dequeue());
                }
            }
        }
    }
}