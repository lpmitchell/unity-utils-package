using System;
using LpmGames.Utils.ControlFlow;

// ReSharper disable once CheckNamespace
namespace LpmGames.Utils
{
    [Serializable]
    public class Boxed<T>
    {
        public delegate void ChangeDelegate(Boxed<T> boxedValue, T previousValue);
        
        /// <summary>
        /// Fired when the value is changed. By default the ChangeEqualityCheck is set to true which means that this
        /// event will only fire when a new value is set in this boxed instance that differs from the previously set
        /// value. In some cases you may want ti disable the ChangeEqualityCheck, in which case the Changed event will
        /// be fired any time a value is set to the Value property.
        /// </summary>
        public event ChangeDelegate Changed;
        
        /// <summary>
        /// Set to false to disable the equality check that happens before the value is changed, if the equality
        /// check is disabled then the Changed event will not be fired, and any WaitForChange yield instructions will
        /// not continue if the value is attempted to be changed to something equal to the current value.
        /// </summary>
        public bool ChangeEqualityCheck = true;
        
        private WaitForResult<T> _waitForChange;
        private T _value;
        
        public T Value
        {
            get => _value;
            set
            {
                if (ChangeEqualityCheck && _value != null && _value.Equals(value)) return;
                
                var prev = _value;
                _value = value;
                Changed?.Invoke(this, prev);
                
                if (_waitForChange != null)
                {
                    _waitForChange.Continue(value);
                    _waitForChange = null;
                }
            }
        }
        
        public Boxed(T initialValue)
        {
            _value = initialValue;
        }
        
        public Boxed()
        {
            _value = default;
        }
        
        /// <summary>
        /// Returns a yield instruction that will pause a coroutine until the boxed value changes
        /// </summary>
        /// <returns>Custom yield instruction</returns>
        public WaitForResult<T> WaitForChange()
        {
            if (_waitForChange != null) return _waitForChange;
            _waitForChange = new WaitForResult<T>();
            return _waitForChange;
        }
    }
}