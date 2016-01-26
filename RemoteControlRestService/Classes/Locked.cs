using System.Threading;

namespace RemoteControlRestService.Classes
{
    public class Locked<T>
        where T : class
    {
        ReaderWriterLockSlim _Locker;

        T _Value;
        public T Value
        {
            get
            {
                _Locker.EnterReadLock();

                try
                {
                    return _Value;
                }
                finally
                {
                    _Locker.ExitReadLock();
                }
            }
            set
            {
                _Locker.EnterWriteLock();

                try
                {
                    _Value = value;
                }
                finally
                {
                    _Locker.ExitWriteLock();
                }
            }
        }

        public Locked(T value)
        {
            _Locker = new ReaderWriterLockSlim();
            _Value = value;
        }
    }
}
