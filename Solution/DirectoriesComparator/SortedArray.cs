using System;
using System.Collections.Generic;
using System.Runtime;

namespace DirectoriesComparator
{
   public partial class SortedArray<T>
   {
      public int Count { get; private set; }

      public int Capacity
      {
         get { return m_InternalArray.Length; }
      }

      private readonly Comparison<T> m_Comparer;
      private T[] m_InternalArray;
      private int m_DataStartIndex;
      private int LeftMargin { get; set; }

      private int RightMargin
      {
         get { return m_InternalArray.Length; }
      }


      public SortedArray( ICollection<T> p_Collection, Comparison<T> p_Comparer = null )
      {
         if (p_Collection == null)
            throw new ArgumentNullException("p_Collection");

         m_Comparer = p_Comparer ?? Comparer<T>.Default.Compare;

         Count = 0;
         SetCapacity(p_Collection.Count != 0 ? p_Collection.Count : 1);

         var i = 0;
         foreach (var item in p_Collection)
         {
            m_InternalArray[i] = item;
            ++i;
         }
         Count = i;
         m_DataStartIndex = 0;
         Array.Sort(m_InternalArray, m_Comparer);
      }


      public SortedArray( int p_Capacity = 1, Comparison<T> p_Comparer = null )
      {
         if (p_Capacity <= 0)
            throw new ArgumentOutOfRangeException("p_Capacity");

         m_Comparer = p_Comparer ?? Comparer<T>.Default.Compare;

         Count = 0;
         SetCapacity(p_Capacity);
      }


      public void Add(T p_NewItem)
      {
         if (Count == m_InternalArray.Length)
         {
            SetCapacity(m_InternalArray.Length * 2);
         }

         InsertItemInternal(p_NewItem);

         InvalidateDependentCollections();
      }

      public void SetCapacity(int p_NewCapacity)
      {
         if (p_NewCapacity < Count)
            throw new ArgumentException("p_NewCapacity");

         if (p_NewCapacity == Count)
            return;


         var tmpArray = new T[p_NewCapacity];

         if (Count != 0)
         {
            if (DataEndIndex <= p_NewCapacity)
            {
               Array.Copy(m_InternalArray, m_DataStartIndex, tmpArray, m_DataStartIndex, Count);
            }
            else
            {
               var newSpace = p_NewCapacity - Count;
               var destinationIndex = newSpace / 2;
               Array.Copy(m_InternalArray, m_DataStartIndex, tmpArray, destinationIndex, Count);
               m_DataStartIndex = destinationIndex;
            }
         }
         else
         {
            m_DataStartIndex = p_NewCapacity / 2;
         }

         LeftMargin = -1;
         m_InternalArray = tmpArray;
      }

      public IEnumerable<T> GetRange(T p_BeginMargin, T p_EndMargin)
      {
         var compareResult = m_Comparer(p_BeginMargin, p_EndMargin);

         var beginMargin = compareResult <= 0 ? p_BeginMargin : p_EndMargin;
         var endMargin = compareResult <= 0 ? p_EndMargin : p_BeginMargin;

         var startIndex = GetIndex(beginMargin, Direction.Left);
         var endIndex = GetIndex(endMargin, Direction.Right);

         var result = Collection.Create(this, startIndex, endIndex);
         return result;
      }

      public IEnumerable<T> GetAll()
      {
         var startIndex = m_DataStartIndex;
         var endIndex = DataEndIndex;

         var result = Collection.Create(this, startIndex, endIndex);
         return result;
      }

      public ICollection<ICollection<T>> GetEqualItemsGroups()
      {

         if (Count== 0)
         {
            return new List<ICollection<T>>(0);
         }

         var result = new List<ICollection<T>>();

         ICollection<T> group;
         if (Count == 1)
         {
            group = new T[1];
            ((T[])group)[0] = m_InternalArray[m_DataStartIndex];
            result.Add( group );
            return result;
         }

         var firstItemIndexOfGroup = m_DataStartIndex;
         var index = firstItemIndexOfGroup + 1;
         var lastItemIndexOfGroup = firstItemIndexOfGroup;
         while (index <= DataEndIndex)
         {
            if (m_Comparer(m_InternalArray[index], m_InternalArray[firstItemIndexOfGroup]) != 0)
            {
               group = Collection.Create(this, firstItemIndexOfGroup, lastItemIndexOfGroup);
               result.Add(group);

               firstItemIndexOfGroup = index;
            }

            lastItemIndexOfGroup = index;
            ++index;
         }
         if (firstItemIndexOfGroup != index)
         {
            group = Collection.Create( this, firstItemIndexOfGroup, lastItemIndexOfGroup );
            result.Add( group );
         }

         return result;
      }

      private int GetIndex(T p_Item, Direction p_RangeBoundary)
      {
         var rightMargin = DataEndIndex;
         var leftMargin = m_DataStartIndex;

         while ((rightMargin - leftMargin) > 1)
         {
            var middle = (rightMargin + leftMargin) / 2;

            var comparisonMiddleResult = m_Comparer(p_Item, m_InternalArray[middle]);

            if (comparisonMiddleResult == 0)
               return middle;

            if (comparisonMiddleResult < 0)
               rightMargin = middle;

            if (comparisonMiddleResult > 0)
               leftMargin = middle;
         }

         var comparisonRightResult = m_Comparer(p_Item, m_InternalArray[rightMargin]);
         var comparisonLeftResult = m_Comparer(p_Item, m_InternalArray[leftMargin]);
         switch (p_RangeBoundary)
         {
            case Direction.Left:
               {
                  if (comparisonLeftResult > 0)
                  {
                     //в случае, если искомый элемент больше крайнего правого элемента массива, то указываем за границу данных
                     return comparisonRightResult > 0 ? rightMargin + 1 : rightMargin;
                  }
                  else
                     return leftMargin;
               }

            case Direction.Right:
               {
                  if (comparisonLeftResult < 0)
                     return leftMargin - 1;
                  else
                     return comparisonRightResult < 0 ? leftMargin : rightMargin;
               }

            default:
               throw new ArgumentOutOfRangeException("p_RangeBoundary");
         }

      }

      private int LeftSpace
      {
         get { return m_DataStartIndex - LeftMargin - 1; }
      }

      private int RightSpace
      {
         get { return RightMargin - DataEndIndex - 1; }
      }

      private int DataEndIndex
      {
         get { return m_DataStartIndex + Count - 1; }
      }


      private void InsertItemInternal(T p_NewItem)
      {
         int newItemIndex;
         switch (Count)
         {
            case 0:
               newItemIndex = m_DataStartIndex;
               break;

            case 1:
               var comparisonResult = m_Comparer(p_NewItem, m_InternalArray[m_DataStartIndex]);

               if (comparisonResult < 0) //новый элемент меньше имеющегося => вставляем слева
               {
                  if (LeftSpace == 0)
                     m_InternalArray[DataEndIndex + 1] = m_InternalArray[m_DataStartIndex];
                  else
                     --m_DataStartIndex;

                  newItemIndex = m_DataStartIndex;
               }
               else //новый элемент больше либо равен имеющемуся => вставляем справа
               {
                  if (RightSpace != 0)
                     newItemIndex = DataEndIndex + 1;
                  else
                  {
                     m_InternalArray[m_DataStartIndex - 1] = m_InternalArray[m_DataStartIndex];
                     --m_DataStartIndex;
                     newItemIndex = DataEndIndex;
                  }
               }
               break;

            default:
               newItemIndex = GetIndexForNewItem(p_NewItem);
               newItemIndex = CorrectArrayAndItemIndex(newItemIndex);
               break;
         }

         m_InternalArray[newItemIndex] = p_NewItem;
         ++Count;
      }

      private int CorrectArrayAndItemIndex(int p_NewItemIndex)
      {
         int result = p_NewItemIndex;

         if (p_NewItemIndex >= DataEndIndex)
               //индекс нового элемента больше либо равен правой границе данных
         {
            if (RightSpace != 0)
               result = DataEndIndex + 1;
            else
            {
               var startIndex = DataEndIndex;
               var movingDistance = LeftSpace / 2;
               movingDistance = movingDistance > 0 ? movingDistance : 1;

               MoveArrayItems(Direction.Left, startIndex, movingDistance);
               m_DataStartIndex -= movingDistance;
               result = DataEndIndex + 1;
            }
         }
         else if (p_NewItemIndex <= m_DataStartIndex)
               //индекс нового элемента меньше либо равен левой границе данных
         {
            int startMovingIndex;
            if (p_NewItemIndex == m_DataStartIndex)
            {
               if (LeftSpace == 0)
               {
                  //Перемещаем правую (б0льшую) часть массива, затем перемещаем оставшийся элемент
                  startMovingIndex = m_DataStartIndex + 1;
                  var movingDistance = RightSpace / 2;
                  movingDistance = movingDistance > 0 ? movingDistance : 1;

                  MoveArrayItems(Direction.Right, startMovingIndex, movingDistance);
                  if (movingDistance > 1)
                  {
                     var newDataStartIndex = m_DataStartIndex + movingDistance - 1;
                     m_InternalArray[newDataStartIndex] = m_InternalArray[m_DataStartIndex];

                     m_DataStartIndex = newDataStartIndex;
                  }
                  result = p_NewItemIndex + movingDistance;
               }
               else
               {
                  var newDataStartIndex = m_DataStartIndex - 1;
                  m_InternalArray[newDataStartIndex] = m_InternalArray[m_DataStartIndex];
                  m_DataStartIndex = newDataStartIndex;
               }
            }
            else
            {
               if (LeftSpace == 0)
               {
                  startMovingIndex = m_DataStartIndex;
                  var movingDistance = RightSpace / 2;
                  movingDistance = movingDistance > 0 ? movingDistance : 1;

                  MoveArrayItems(Direction.Right, startMovingIndex, movingDistance);
                  m_DataStartIndex += movingDistance;
               }
               else
                  ; //do nothing

               result = m_DataStartIndex - 1;
               m_DataStartIndex = result;
            }
         }
         else
         {
            //индекс нового элемента лежит между правой и левой границей данных в массиве
            var lengthLeftData = p_NewItemIndex - m_DataStartIndex;
            var lengthRightData = DataEndIndex - p_NewItemIndex;

            Direction direction;

            if (lengthLeftData < lengthRightData)
            {
               direction = LeftSpace != 0 ? Direction.Left : Direction.Right;
            }
            else if (lengthRightData > lengthLeftData)
            {
               direction = RightSpace != 0 ? Direction.Right : Direction.Left;
            }
            else
                  //if (lengthRight == lengthLeft)
            {
               direction = LeftSpace >= RightSpace ? Direction.Left : Direction.Right;
            }

            result = direction == Direction.Right ? p_NewItemIndex + 1 : p_NewItemIndex;

            MoveArrayItems(direction, result, 1);
            if (direction == Direction.Left)
            {
               m_DataStartIndex -= 1;
            }
         }
         return result;
      }

      /// <summary>
      /// Возвращает индекс для нового элемента.
      /// Для непустого массива результат указывает либо на пустой элемент массива (не занятую данными позицию),
      /// либо на элемент меньший или равный новому элементу.
      /// - 
      /// Например, для массива A{1,3} и нового элемента i{2} возвращаемый индекс будет указывать на элемент a{1},
      /// 
      /// true = ((A[i] equ a) || (A[i] lt a))
      ///                                    || (A[i] == null)
      /// </summary>
      /// <param name="p_Item"></param>
      /// <returns></returns>
      private int GetIndexForNewItem(T p_Item)
      {
         var leftMargin = m_DataStartIndex;
         var rightMargin = DataEndIndex;

         var comparisonLeftResult = m_Comparer(p_Item, m_InternalArray[leftMargin]);
         if (comparisonLeftResult == 0)
            return leftMargin;

         var comparisonRightResult = m_Comparer(p_Item, m_InternalArray[rightMargin]);
         if (comparisonRightResult == 0)
            return rightMargin;

         while (true)
         {
            if (comparisonLeftResult < 0)
               return leftMargin - 1;

            if (comparisonRightResult > 0)
               return rightMargin + 1;

            var dataLen = rightMargin - leftMargin + 1;
            switch (dataLen)
            {
               case 1:
                  return leftMargin;

               case 2:
                  return comparisonRightResult >= 0 ? rightMargin : leftMargin;

               default:
                  var middle = (rightMargin + leftMargin) / 2;

                  var comparisonMiddleResult = m_Comparer(p_Item, m_InternalArray[middle]);
                  if (comparisonMiddleResult != 0)
                  {
                     if (comparisonMiddleResult < 0)
                     {
                        rightMargin = middle;
                        comparisonRightResult = comparisonMiddleResult;
                     }
                     else if (comparisonMiddleResult > 0)
                     {
                        leftMargin = middle;
                        comparisonLeftResult = comparisonMiddleResult;
                     }
                  }
                  else
                  {
                     return middle; //здесь широкое поле для оптимизации
                  }
                  break;
            }
         }
      }

      private enum Direction
      {
         Left,
         Right
      }

      [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
      private void MoveArrayItems(Direction p_Direction, int p_StartIndex, int p_Distanse)
      {
         int sourceIndex;
         int destinationIndex;
         int length;

         switch (p_Direction)
         {
            case Direction.Left:
               length = p_StartIndex - m_DataStartIndex + 1;
               sourceIndex = m_DataStartIndex;
               destinationIndex = m_DataStartIndex - p_Distanse;
               break;

            case Direction.Right:
               length = DataEndIndex - p_StartIndex + 1;
               sourceIndex = p_StartIndex;
               destinationIndex = p_StartIndex + p_Distanse;
               break;

            default:
               throw new ArgumentOutOfRangeException("p_Direction");
         }

         Array.Copy(m_InternalArray, sourceIndex, m_InternalArray, destinationIndex, length);
      }
   }
}
