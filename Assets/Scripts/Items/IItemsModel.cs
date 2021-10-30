using System;

namespace Mans
{
    public interface IItemsModel<T>
    {
        event Action<T,bool> EvtAddItem;
        event Action<T> EvtRemoveItem;
        void AddItem(int ID);
        void RemoveItem(int ID);
        bool HaveEffect(int ID);
    }
}