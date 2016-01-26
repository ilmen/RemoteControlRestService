using System;
using System.Collections.Generic;

namespace RemoteControlRestService.Classes
{
    public interface ICRUDCollection<TPrimary, TItem>
    {
        void Delete(TPrimary id);
        IEnumerable<TItem> GetAll();
        TItem GetOne(TPrimary id);
        void Insert(TItem value);
        void Update(TPrimary id, TItem value);
    }
}