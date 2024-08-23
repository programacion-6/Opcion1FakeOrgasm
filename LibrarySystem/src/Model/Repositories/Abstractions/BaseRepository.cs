// namespace LibrarySystem;

// public abstract class BaseRepository<T> : IRepository<T> where T : EntityBase
// {
//     protected Dictionary<Guid, T> Data = new Dictionary<Guid, T>();

//     public bool Save(T item)
//     {
//         bool wasSaved = false;
//         if (!Data.ContainsKey(item.Id))
//         {
//             Data[item.Id] = item;
//             wasSaved = true;
//         }
//         return wasSaved;
//     }

//     public bool Update(T item)
//     {
//         bool wasUpdated = false;
//         if (Data.ContainsKey(item.Id))
//         {
//             Data[item.Id] = item;
//             wasUpdated = true;
//         }

//         return wasUpdated;
//     }

//     public bool Delete(T item)
//     {
//         bool wasDeleted = false;

//         if (Data.ContainsKey(item.Id))
//         {
//             wasDeleted = Data.Remove(item.Id);
//         }

//         return wasDeleted;
//     }

//     public T? GetById(Guid itemId)
//     {
//         return Data.TryGetValue(itemId, out T? value) ? value : null;
//     }

//     public List<T> GetAll()
//     {
//         return [.. Data.Values];
//     }
// }