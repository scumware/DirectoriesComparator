using System;
using System.Collections;
using System.Collections.Generic;

namespace DirectoriesComparator
{
   public partial class SortedArray<T>
   {
      #region Управление связанными коллекциями

      private readonly List<Collection> m_Collections = new List<Collection>();

      private void AddCollection(Collection p_Collection)
      {
         m_Collections.Add(p_Collection);
      }

      private void RemoveCollection(Collection p_Collection)
      {
         m_Collections.Remove(p_Collection);
      }

      private void InvalidateDependentCollections()
      {
         foreach (var collection in m_Collections)
         {
            collection.Invalid = true;
         }
      }

      #endregion

      private sealed class Collection :ICollection<T>, IDisposable
      {
         private bool m_Disposed;
         internal bool Invalid = false;

         private readonly SortedArray<T> m_Array;

         internal SortedArray<T> Array
         {
            get { return m_Array; }
         }

         private readonly int m_FirstItemIndex;

         internal int FirstItemIndex
         {
            get { return m_FirstItemIndex; }
         }

         private readonly int m_LastItemIndex;

         internal int LastItemIndex
         {
            get { return m_LastItemIndex; }
         }

         public static Collection Create(SortedArray<T> p_Array, int p_StartIndex, int p_EndIndex)
         {
            var collection = new Collection(p_Array, p_StartIndex, p_EndIndex);
            p_Array.AddCollection(collection);
            return collection;
         }


         private Collection(SortedArray<T> p_Array, int p_FirstItemIndex, int p_LastItemIndex)
         {
            m_Array = p_Array;
            m_FirstItemIndex = p_FirstItemIndex;
            m_LastItemIndex = p_LastItemIndex;
         }

         public IEnumerator<T> GetEnumerator()
         {
            ValidateAccessing();
            return new Enumerator(this);
         }

         IEnumerator IEnumerable.GetEnumerator()
         {
            return GetEnumerator();
         }

         public void Add(T p_Item)
         {
            throw new NotSupportedException();
         }

         public void Clear()
         {
            throw new NotSupportedException();
         }

         public bool Contains(T p_Item)
         {
            ValidateAccessing();
            //реализоать бинарный поиск элемента
            throw new NotImplementedException();
         }

         public void CopyTo(T[] p_Array, int p_ArrayIndex)
         {
            ValidateAccessing();

            System.Array.Copy(m_Array.m_InternalArray, m_FirstItemIndex, p_Array, p_ArrayIndex, Count);
         }

         public bool Remove(T p_Item)
         {
            throw new NotSupportedException();
         }

         public int Count
         {
            get { return m_LastItemIndex - m_FirstItemIndex + 1; }
         }

         public bool IsReadOnly
         {
            get { return true; }
         }

         public void Dispose()
         {
            if (!m_Disposed)
            {
               m_Array.RemoveCollection(this);
            }
            m_Disposed = true;
         }

         internal void ValidateAccessing()
         {
            if (Invalid || m_Disposed)
            {
               throw new InvalidOperationException();
            }
         }
      }

      private sealed class Enumerator :IEnumerator<T>
      {
         private readonly Collection m_SourceCollection;
         private int m_CurrentIndex;
         private bool m_Disposed;
         private bool m_FirstItemGotten = false;

         public Enumerator(Collection p_SourceCollection)
         {
            m_SourceCollection = p_SourceCollection;
            m_CurrentIndex = p_SourceCollection.FirstItemIndex;
         }

         public void Dispose()
         {
            if (m_Disposed)
               return;

            m_Disposed = true;
         }

         public bool MoveNext()
         {
            if (m_Disposed)
               throw new InvalidOperationException();

            m_SourceCollection.ValidateAccessing();

            if (!m_FirstItemGotten)
            {
               m_CurrentIndex = m_SourceCollection.FirstItemIndex;
               m_FirstItemGotten = true;
            }
            else
            {
               ++m_CurrentIndex;
            }
            return m_CurrentIndex <= m_SourceCollection.LastItemIndex;
         }

         public void Reset()
         {
            if (m_Disposed)
               throw new InvalidOperationException();

            m_SourceCollection.ValidateAccessing();

            m_FirstItemGotten = false;
         }

         public T Current
         {
            get
            {
               if (m_Disposed)
                  throw new InvalidOperationException();

               m_SourceCollection.ValidateAccessing();

               return m_SourceCollection.Array.m_InternalArray[m_CurrentIndex];
            }
         }

         object IEnumerator.Current
         {
            get { return Current; }
         }
      }
   }
}