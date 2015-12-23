using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlRestService.Infrastracture
{
    public class SimleObservableCollection<T> : IList<T>
    {
        List<T> items = new List<T>();

        public  delegate void EventHandler(Object sender, CollectionChangedEventArgs<T> e);
        public event EventHandler CollectionChanged;

        public SimleObservableCollection()
        {
            CollectionChanged += (s, e) => { /* default event handler */ };
        }

        public void Insert(int index, T item)
        {
            items.Insert(index, item);

            CollectionChanged(this, CollectionChangedEventArgs<T>.CreateAddedEvent(item));
        }

        public void Add(T item)
        {
            items.Add(item);

            CollectionChanged(this, CollectionChangedEventArgs<T>.CreateAddedEvent(item));
        }

        public void RemoveAt(int index)
        {
            var oldItem = items[index];

            items.RemoveAt(index);

            CollectionChanged(this, CollectionChangedEventArgs<T>.CreateRemovedEvent(oldItem));
        }

        public bool Remove(T item)
        {
            var succeeded =  items.Remove(item);

            CollectionChanged(this, CollectionChangedEventArgs<T>.CreateRemovedEvent(item));

            return succeeded;
        }

        public T this[int index]
        {
            get
            {
                return items[index];
            }

            set
            {
                var oldItem = items[index];
                var newItem = value;

                items[index] = newItem;

                CollectionChanged(this, CollectionChangedEventArgs<T>.CreateRemovedEvent(oldItem));
                CollectionChanged(this, CollectionChangedEventArgs<T>.CreateAddedEvent(newItem));
            }
        }
        
        #region not implemented methods
		public void Clear()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        } 
	    #endregion

        #region unmutable methods
        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        } 
        #endregion
    }

    public class CollectionChangedEventArgs<T> : EventArgs
    {
        public enChanges Action
        { get; private set; }

        public T NewItem
        { get; private set; }

        public T OldItem
        { get; private set; }

        CollectionChangedEventArgs (enChanges action, T oldItem, T newItem)
	    {
            Action = action;
            OldItem = oldItem;
            NewItem = newItem;
	    }

        public static CollectionChangedEventArgs<T> CreateAddedEvent(T newItem)
        {
            return new CollectionChangedEventArgs<T>(enChanges.Added, default(T), newItem);
        }

        public static CollectionChangedEventArgs<T> CreateRemovedEvent(T oldItem)
        {
            return new CollectionChangedEventArgs<T>(enChanges.Removed, oldItem, default(T));
        }
    }

    public enum enChanges 
    {
        Added = 0,
        Removed = 1,
    }
}
