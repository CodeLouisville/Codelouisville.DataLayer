using System;
using System.Collections.Generic;

namespace CodeLouisville.DL.Interfaces
{
    public interface IData<T>
    {
        void Save(IList<T> objCollection);

        void SaveOne(T obj, Func<IList<T>, int> query);

        IList<T> Get();

        T GetByQuery(Func<IList<T>, T> query);
    }
}