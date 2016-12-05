using System.Collections.Generic;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IServise<T>
    {
        void Create(T item);

        void Update(T item);

        void Delete(T item);

        T GetById(int id);

        List<T> GetAll();
    }
}
