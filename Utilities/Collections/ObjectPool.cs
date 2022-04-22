using System;
using System.Collections.Concurrent;

namespace Plus.Utilities.Collections
{
    public sealed class ObjectPool<T>
    {
        private readonly ConcurrentBag<T> _objects;
        private readonly Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            if (objectGenerator == null) { /*TODO ExceptionLogger.LogCriticalException("ObjectGenerator was null");*/ }
            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator;
        }

        public T GetObject()
        {
            T item;
            if (_objects.TryTake(out item)) { return item; }
            return _objectGenerator();
        }

        public void PutObject(T item)
        {
            _objects.Add(item);
        }
    }
}
