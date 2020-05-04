using UnityEngine;

// ReSharper disable once CheckNamespace
namespace LpmGames.Utils.ControlFlow
{
    public class WaitForResult<T> : CustomYieldInstruction
    {
        private readonly bool _hasTimeout;
        private readonly float _timeoutTime;
        public T Result { get; private set; }
        private bool _filled;

        public override bool keepWaiting => !_filled && (!_hasTimeout || _timeoutTime > Time.time);

        public WaitForResult(float timeout = 0)
        {
            _hasTimeout = timeout > 0;
            _timeoutTime = Time.time + timeout;
        }
        
        public void Continue(T result)
        {
            Result = result;
            _filled = true;
        }
    }
}