﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

namespace EntryEngine
{
    public static partial class Utility
    {
        public static string ALPHABET = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string NumberToCode(long value, int digit)
        {
            return NumberToCode(value, digit, (long)Math.Pow(ALPHABET.Length, digit + 1) >> 1);
        }
        public static string NumberToCode(long value, int digit, long start)
        {
            long v = value + start;
            char[] chars = new char[digit];
            int len = ALPHABET.Length;
            for (int i = digit - 1; i >= 0; i--)
            {
                chars[i] = ALPHABET[(int)(v % len)];
                v /= len;
            }
            return new string(chars);
        }
        public static long CodeToNumber(string code)
        {
            return CodeToNumber(code, (long)Math.Pow(ALPHABET.Length, code.Length + 1) >> 1);
        }
        public static long CodeToNumber(string code, long start)
        {
            long value = 0;
            long len = 1;
            for (int i = code.Length - 1; i >= 0; i--)
            {
                value += ALPHABET.IndexOf(code[i]) * len;
                len *= ALPHABET.Length;
            }
            return value - start;
        }

		public static string BinaryToHex(byte[] binary)
		{
			if (binary == null)
				return "";
			const string HEX = "0123456789ABCDEF";
			char[] chars = new char[binary.Length * 2];
			for (int i = 0; i < binary.Length; ++i)
			{
				chars[i * 2] = HEX[binary[i] >> 4];
				chars[i * 2 + 1] = HEX[binary[i] & 0xf];
			}
			return new string(chars);
		}
        /// <summary>显示指定小数点位数的float值，不足的位数会用0补齐</summary>
		/// <param name="value">值</param>
		/// <param name="length">要显示的小数点后面的位数</param>
		/// <returns>指定位数的float字符串</returns>
		public static string LengthFloat(float value, int length)
		{
            string v = value.ToString();
            int sep = v.IndexOf('.');
            if (sep == -1)
                return v;
            int l = sep + 1 + length;
            if (v.Length < l)
            {
                for (int i = v.Length; i < l; i++)
                    v += "0";
                return v;
            }
            else
                return v.Substring(0, l);
		}
        public static string ToPercent(float value)
        {
            string str = ToDisplay(value);
            if (!string.IsNullOrEmpty(str))
                return str + "%";
            return str;
            //return value.ToString("0.00%");
        }
		public static string ToDisplay(float value)
		{
            if (float.IsNaN(value))
                return string.Empty;
            string str = (value * 100).ToString();
            int index = str.IndexOf('.');
            if (index != -1 && str.Length > index + 3)
                str = str.Substring(0, index + 3);
            return str;
            //return value.ToString("0.00");
		}
        public static int GetNumber(string str, int index)
        {
            int value = 0;
            int temp;
            for (int i = 0; ; i++)
            {
                temp = index + i;
                if (temp >= str.Length || str[temp] < '0' || str[temp] > '9')
                    break;
                value = value * 10 + (int)(str[temp] - '0');
            }
            return value;
        }


		#region Array Expand Method


        public static T[] ToArray<T>(T value, int size)
        {
            T[] array = new T[size];
            for (int i = 0; i < array.Length; i++)
                array[i] = value;
            return array;
        }
        public static T[] ToArrayAfterCount<T>(this IEnumerable<T> array)
        {
            T[] result;

            int count;
            ICollection<T> collection = array as ICollection<T>;
            if (collection != null)
            {
                count = collection.Count;
                result = new T[count];
                if (count > 0)
                    collection.CopyTo(result, 0);
                return result;
            }
            else
            {
                System.Collections.ICollection collection2 = array as System.Collections.ICollection;
                if (collection2 != null)
                {
                    count = collection2.Count;
                    result = new T[count];
                    collection2.CopyTo(result, 0);
                    return result;
                }
                else
                {
                    count = 0;
                    foreach (var item in array)
                        count++;
                    result = new T[count];

                    count = 0;
                    foreach (var item in array)
                        result[count++] = item;
                    return result;
                }
            }
        }
        public static T[] Add<T>(this T[] array, params T[] items)
        {
            if (array == null)
            {
                array = new T[items.Length];
                items.Copy(0, array, 0, items.Length);
                return array;
            }
            else
            {
                return array.Insert(array.Length, items);
            }
        }
		public static int BinarySearch<T>(this IList<T> array, T target, Comparison<T> comparer)
		{
			return array.BinarySearch(target, 0, array.Count, comparer);
		}
		public static int BinarySearch<T>(this IList<T> array, T target, int index, int length, Comparison<T> comparer)
		{
			int i = index;
			int num = index + length - 1;
			while (i <= num)
			{
				int median2 = i + (num - i >> 1);
				int num3 = comparer(array[median2], target);

				if (num3 == 0)
				{
					return median2;
				}
				if (num3 < 0)
				{
					i = median2 + 1;
				}
				else
				{
					num = median2 - 1;
				}
			}
			return -1;
		}
        public static List<U> Cast<T, U>(this IList<T> list, Func<T, U> ret)
        {
            List<U> results = new List<U>(list.Count);
            foreach (var item in list)
                results.Add(ret(item));
            return results;
        }
		public static List<T> WhereAndRemove<T>(this IEnumerable<T> array, Func<T, bool> method, out List<T> other)
		{
			List<T> results = new List<T>();
			other = new List<T>();
			foreach (T item in array)
			{
				if (method(item))
				{
					results.Add(item);
				}
				else
				{
					other.Add(item);
				}
			}
			return results;
		}
		public static int IndexOf<T>(this IEnumerable<T> array, T target)
		{
			return IndexOf(array, target, (p1, p2) => { return p1.Equals(p2); });
		}
		public static int IndexOf<T, U>(this IEnumerable<T> array, T target, Func<T, U> comparer)
		{
			int index = 0;
			U temp = comparer(target);
			foreach (T item in array)
			{
				if (comparer(item).Equals(temp))
				{
					return index;
				}
				index++;
			}
			return -1;
		}
		public static int IndexOf<T>(this IEnumerable<T> array, T target, Func<T, T, bool> comparer)
		{
			int index = 0;
			foreach (T item in array)
			{
				if (comparer(item, target))
				{
					return index;
				}
				index++;
			}
			return -1;
		}
		public static int IndexOf<T>(this IEnumerable<T> array, Func<T, bool> comparer)
		{
			int index = 0;
			foreach (T item in array)
			{
				if (comparer(item))
				{
					return index;
				}
				index++;
			}
			return -1;
		}
		public static int IndexOf<T>(this T[] array, params T[] target)
		{
			return IndexOf(array, 0, array.Length, target);
		}
        public static int IndexOf<T>(this T[] array, int index, int count, params T[] target)
        {
            return IndexOf(array, index, count, (p1, p2) =>
            {
                if (p1 == null)
                    return p2 == null;
                else
                    return p1.Equals(p2);
            }, target);
        }
		public static int IndexOf<T, U>(this T[] array, int index, int count, Func<T, U> comparer, T[] target)
		{
			if (target.Length == 0)
				return -1;

			U temp = comparer(target[0]);
			int end = _MATH.Min(index + count, array.Length);
			for (int i = index; i < end; i++)
			{
				if (comparer(array[i]).Equals(temp))
				{
					if (target.Length == 1)
						return i;
					if (target.Length + i >= end)
						return -1;
					int j;
					for (j = 1; j < target.Length; j++)
					{
						if (!comparer(array[i + j]).Equals(target[j]))
						{
							break;
						}
					}
					if (j == target.Length)
					{
						return i;
					}
				}
			}
			return -1;
		}
        public static int IndexOf<T>(this T[] array, int index, int count, Func<T, T, bool> comparer, params T[] target)
		{
			if (target.Length == 0)
				return -1;

			int end = _MATH.Min(index + count, array.Length);
			for (int i = index; i < end; i++)
			{
				if (comparer(array[i], target[0]))
				{
					if (target.Length == 1)
						return i;
					if (target.Length + i > end)
						return -1;
					int j;
					for (j = 1; j < target.Length; j++)
					{
						if (!comparer(array[i + j], target[j]))
						{
							break;
						}
					}
					if (j == target.Length)
					{
						return i;
					}
				}
			}
			return -1;
		}
        public static T[] Insert<T>(this T[] array, int index, params T[] items)
        {
            if (array == null)
                array = new T[0];
            int length = items.Length;
            T[] clone = new T[array.Length + length];
            array.Copy(0, clone, 0, index);
            items.Copy(0, clone, index, length);
            length += index;
            array.Copy(index, clone, length, clone.Length - length);
            return clone;
        }
		public static void Copy<T>(this T[] reff, int fromIndex, T[] target, int toIndex, int length)
		{
			Array.Copy(reff, fromIndex, target, toIndex, length);
			//for (int i = fromIndex, j = toIndex; j < toIndex + length; i++, j++)
			//    target[j] = reff[i];
		}
		public static bool Contains<T, U>(this IEnumerable<T> array, T target, Func<T, U> comparer)
		{
			return IndexOf(array, target, comparer) != -1;
		}
        public static bool Contains<T>(this IEnumerable<T> array, T target, Func<T, T, bool> comparer)
		{
			return IndexOf(array, target, comparer) != -1;
		}
		public static IEnumerable<T> Current<T>(this IEnumerable<T> current, IEnumerable<T> previous, Func<T, T, bool> comparer)
		{
			foreach (var item1 in current)
			{
                bool flag = false;
				foreach (var item2 in previous)
				{
					if (comparer(item1, item2))
					{
						yield return item2;
                        flag = true;
                        break;
					}
				}
                if (!flag)
				    yield return item1;
			}
		}
		public static IEnumerable<T> Current<T>(this IEnumerable<T> current, IEnumerable<T> previous)
		{
			return Current(current, previous, (t1, t2) => t1.Equals(t2));
		}
        public static IEnumerable<T> Distinct<T, U>(this IEnumerable<T> array, Func<T, U> comparer)
        {
            HashSet<U> set = new HashSet<U>();
            foreach (var item in array)
                if (set.Add(comparer(item)))
                    yield return item;
        }
		public static bool Equals(this byte[] b1, byte[] b2)
		{
			if (b1 == b2)
				return true;

			if (b1 == null || b2 == null)
				return false;

			int count = b1.Length;
			if (count != b2.Length)
				return false;

            for (int i = 0; i < b1.Length; i++)
                if (b1[i] != b2[i])
                    return false;

			return true;
		}
        public static IEnumerator<T> Enumerator<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.GetEnumerator();
        }
        public static IEnumerable<T> Enumerable<T>(this IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
                yield return item;
        }
        public static IEnumerable<T> Enumerable<T>(this Action action)
        {
            action();
            yield break;
        }
        public static void ForFirstToLast<T>(this LinkedList<T> list, Action<T> action)
        {
            var node = list.First;
            while (node != null)
            {
                var temp = node.Value;
                node = node.Next;
                action(temp);
            }
        }
        public static void ForLastToFirst<T>(this LinkedList<T> list, Action<T> action)
        {
            var node = list.Last;
            while (node != null)
            {
                var temp = node.Value;
                node = node.Previous;
                action(temp);
            }
        }
        public static void Foreach<T>(this IEnumerable<T> array)
        {
            foreach (var item in array) { }
        }
        public static List<T> Foreach<T>(this List<T> list, Action<T> action)
        {
            foreach (var item in list)
                if (action != null)
                    action(item);
            return list;
        }
		public static void ForeachExceptLast<T>(this IEnumerable<T> array, Action<T> all, Action<T> except)
		{
			bool flag = false;
			foreach (var item in array)
			{
				if (except != null && flag)
					except(item);
				if (all != null)
					all(item);
				flag = true;
			}
		}
		public static bool IsEmpty<T>(this T[] array)
		{
			return array == null || array.Length == 0;
		}
        public static T MaxOrDefault<T>(this IEnumerable<T> source, Func<T, int> selector)
        {
            return MaxMin(source, selector, false);
        }
        public static T MinOrDefault<T>(this IEnumerable<T> source, Func<T, int> selector)
        {
            return MaxMin(source, selector, true);
        }
		public static T MaxMin<T>(this IEnumerable<T> source, Func<T, int> selector, bool min)
		{
			int num = 0;
			bool flag = false;
			T result = default(T);
			foreach (T current in source)
			{
				int temp = selector(current);
				if (flag)
				{
					if (temp <= num == min)
					{
						num = temp;
						result = current;
					}
				}
				else
				{
					num = temp;
					flag = true;
					result = current;
				}
			}
			return result;
		}
		public static Dictionary<T, List<U>> Group<T, U>(this IEnumerable<U> array, Func<U, T> groupBy)
		{
			Dictionary<T, List<U>> output = new Dictionary<T, List<U>>();
			List<U> value;
			T temp;
            foreach (var item in array)
            {
                temp = groupBy(item);
                if (!output.TryGetValue(temp, out value))
                {
                    value = new List<U>();
                    output.Add(temp, value);
                }
                value.Add(item);
            }
			return output;
		}
        public static T[] GetRange<T>(this T[] array, int start, int count)
		{
			int end = _MATH.Min(start + count, array.Length);
			T[] clone = new T[_MATH.Max(0, end - start)];
			for (int i = start; i < end; i++)
			{
				clone[i - start] = array[i];
			}
			return clone;
		}
        public static T[] GetRange<T>(this T[] array, int start)
		{
			return GetRange(array, start, array.Length - start);
		}
        public static T[] GetArray<T>(this T[] array, int x, int y, int width, int height, int sourceWidth)
        {
            T[] copy = new T[width * height];
            width += x;
            height += y;
            int index = 0;
            for (int j = y; j < height; j++)
            {
                for (int i = x; i < width; i++)
                {
                    copy[index] = array[j * sourceWidth + i];
                    index++;
                }
            }
            return copy;
        }
        public static void SetArray<T>(this T[] array, T[] target, int tx, int ty, int width, int height, int targetWidth, int sourceWidth, int sourceIndex)
        {
            int line = sourceWidth - width;
            height += ty;
            width += tx;
            for (int j = ty; j < height; j++)
            {
                for (int i = tx; i < width; i++)
                {
                    target[j * targetWidth + i] = array[sourceIndex];
                    sourceIndex++;
                }
                sourceIndex += line;
            }
        }
        public static void LoopQueueForward<T>(this IList<T> array)
        {
            int count = array.Count;
            T first = array[0];
            for (int i = 1; i < count; i++)
                array[i - 1] = array[i];
            array[count - 1] = first;
        }
        public static void LoopQueueBackward<T>(this IList<T> array)
        {
            int count = array.Count - 1;
            T last = array[count];
            for (int i = count; i > 0; i--)
                array[i] = array[i - 1];
            array[0] = last;
        }
        public static void LoopQueue<T>(this IList<T> array, bool forward)
        {
            if (forward)
                LoopQueueForward(array);
            else
                LoopQueueBackward(array);
        }
        public static int LastIndex<T>(this ICollection<T> array)
		{
			return array.Count - 1;
		}
        public static int MedianIndex<T>(this ICollection<T> array)
		{
			return array.Count / 2;
		}
		public static T Median<T>(this IList<T> array)
		{
			return array[MedianIndex(array)];
		}
		public static T RandomItem<T>(this IList<T> array)
		{
			return array[_RANDOM.Next(0, array.Count)];
		}
		public static T[] RandomItem<T>(this IList<T> array, int count)
		{
			if (count == 0)
				return new T[count];

			T[] result = new T[count];

			HashSet<int> record = new HashSet<int>();
			int index = 0;
			while (true)
			{
				int temp = _RANDOM.Next(count);
				if (record.Add(temp))
				{
					result[index++] = array[temp];
					if (index == count)
					{
						return result;
					}
				}
			}
		}
		public static T[] RandomItemStep<T>(this IList<T> array, int count)
		{
			if (count == 0)
				return new T[count];

			T[] result = new T[count];

			int max = array.Count;
			int step = _MATH.Ceiling(max * 1.0f / count);
			int index = 0;
			int blockStepMode = count - max / 2;

			for (int i = 0; i < count; i++)
			{
				int temp = _RANDOM.Next(index, index + step);
				if (temp >= max)
					temp = max - 1;
				result[i] = array[temp];
				if (blockStepMode < 0)
				{
					index += step;
				}
				else
				{
					if (blockStepMode != 0)
					{
						if (temp - index == 1)
						{
							blockStepMode--;
							if (blockStepMode == 0)
							{
								step = 1;
							}
						}
					}
					index = temp + 1;
				}
			}

			return result;
		}
		public static T[] RandomItemShuffle<T>(this IEnumerable<T> array, int count)
		{
			T[] result = array.ToArray();
			Shuffle(result);
			return result;
		}
		public static T[] Remove<T>(this T[] array, int index)
		{
			return RemoveRange(array, index, 1);
		}
		public static T[] Remove<T>(this T[] array, Func<T, bool> comparer)
		{
			return Remove(array, comparer, 0, array.Length);
		}
		public static T[] Remove<T>(this T[] array, Func<T, bool> comparer, int index, int count)
		{
			int num = count;
			int num2 = index + count;
            //T _default = default(T);
			for (int i = index; i < num2; i++)
			{
				if (comparer(array[i]))
				{
                    //array[i] = _default;
					num--;
				}
			}

			T[] copy = new T[num];
			num = 0;
			for (int i = index; i < num2; i++)
			{
                if (!comparer(array[i]))
				{
					copy[num++] = array[i];
				}
			}

			return copy;
		}
		public static void RemoveLast<T>(this IList<T> array)
		{
			array.RemoveAt(array.Count - 1);
		}
		public static T[] RemoveRange<T>(this T[] array, int index)
		{
			return RemoveRange(array, index, array.Length - index);
		}
		public static T[] RemoveRange<T>(this T[] array, int index, int count)
		{
			int end = _MATH.Min(index + count, array.Length);
			count = end - index;
			T[] clone = new T[array.Length - count];
			array.Copy(0, clone, 0, index);
			array.Copy(index + count, clone, index, array.Length - end);
			return clone;
		}
		public static void Reverse<T>(this IList<T> array)
		{
			Reverse(array, 0, array.Count);
		}
		public static void Reverse<T>(this IList<T> array, int start, int count)
		{
			T t3;
			int end = _MATH.Min(array.Count, start + count) - 1;
			while (start < end)
			{
				t3 = array[start];
				array[start] = array[end];
				array[end] = t3;
				start++;
				end--;
			}
		}
        public static U SelectFirst<T, U>(this IEnumerable<T> array, Func<T, U> selector)
        {
            T target = array.FirstOrDefault();
            if (target == null)
                return default(U);
            return selector(target);
        }
        public static U SelectLast<T, U>(this IEnumerable<T> array, Func<T, U> selector)
        {
            IList<T> list = array as IList<T>;
            if (list != null)
            {
                if (list.Count == 0)
                    return default(U);
                else
                    return selector(list[list.Count - 1]);
            }

            T target = array.LastOrDefault();
            if (target == null)
                return default(U);
            return selector(target);
        }
        public static void Shuffle<T>(this IList<T> array)
        {
            Shuffle(array, _RANDOM._Random, 0, array.Count);
        }
        public static void Shuffle<T>(this IList<T> array, _RANDOM.Random random)
        {
            Shuffle(array, random, 0, array.Count);
        }
        public static void Shuffle<T>(this IList<T> array, _RANDOM.Random random, int startIndex)
        {
            Shuffle(array, random, startIndex, array.Count);
        }
        /// <summary>对数组里的对象进行洗牌交换位置</summary>
        /// <param name="array">要洗牌的数组</param>
        /// <param name="random">随机种子</param>
        /// <param name="startIndex">洗牌起始索引（随机包含）</param>
        /// <param name="endIndex">洗牌结束索引（随机不包含）</param>
        public static void Shuffle<T>(this IList<T> array, _RANDOM.Random random, int startIndex, int endIndex)
        {
            T temp;
            int index;
            for (int i = startIndex; i < endIndex; ++i)
            {
                index = random.Next(startIndex, endIndex);
                temp = array[index];
                array[index] = array[i];
                array[i] = temp;
            }
        }
		static int Partition<T>(IList<T> array, int low, int high, bool asc, Func<T, int> comparer)
		{
			T temp = array[low];
			int _temp = comparer(temp), compare;

			while (low < high)
			{
				compare = comparer(array[high]);
				if (_temp == compare || _temp < compare == asc)
				{
					high--;
				}
				array[low] = array[high];
				compare = comparer(array[low]);
				if (low < high && (_temp == compare || _temp > compare == asc))
				{
					low++;
				}
				array[high] = array[low];
			}

			array[low] = temp;
			return low;
		}
		public static void SortQuit<T>(this IList<T> array, bool asc, Func<T, int> comparer)
		{
			SortQuit(array, 0, array.Count - 1, asc, comparer);
		}
		public static void SortQuit<T>(this IList<T> array, bool asc, int index, int count, Func<T, int> comparer)
		{
			SortQuit(array, index, _MATH.Min(index + count, array.Count) - 1, asc, comparer);
		}
        /// <summary>快速排序</summary>
		/// <typeparam name="T">排序类型</typeparam>
		/// <param name="array">要排序的数组</param>
		/// <param name="low">排序起始项索引</param>
		/// <param name="high">排序结束项索引</param>
		/// <param name="asc">升序True/降序False</param>
		/// <param name="comparer">要比较的对象代数</param>
		public static void SortQuit<T>(this IList<T> array, int low, int high, bool asc, Func<T, int> comparer)
		{
			int pivotLoc = 0;
			if (low < high)
			{
				pivotLoc = Partition(array, low, high, asc, comparer);
				SortQuit(array, low, pivotLoc - 1, asc, comparer);
				SortQuit(array, pivotLoc + 1, high, asc, comparer);
			}
		}
        /// <summary>稳定排序，同分时顺序不变</summary>
		public static void SortOrderDesc<T>(this IList<T> array, Func<T, int> comparer)
		{
            SortOrder(array, (p1, p2) => comparer(p1) < comparer(p2));
		}
        public static void SortOrderAsc<T>(this IList<T> array, Func<T, int> comparer)
        {
            SortOrder(array, (p1, p2) => comparer(p1) > comparer(p2));
        }
		public static void SortOrder<T>(this IList<T> array, Func<T, T, bool> change)
		{
			int count = array.Count;
			int count2;
			for (int i = 0; i < count; i++)
			{
				count2 = count - i - 1;
				// 冒泡
				for (int j = 0; j < count2; j++)
				{
					if (change(array[j], array[j + 1]))
					{
						T t3 = array[j + 1];
						array[j + 1] = array[j];
						array[j] = t3;
					}
				}
			}
		}
		public static void SortOrderDesc<T>(this IList<T> array) where T : IComparable<T>
		{
			SortOrder(array, (x, y) => x.CompareTo(y) < 0);
		}
        public static void SortOrderAsc<T>(this IList<T> array) where T : IComparable<T>
        {
            SortOrder(array, (x, y) => x.CompareTo(y) > 0);
        }
		public static void Swap<T>(ref T t1, ref T t2)
		{
			T t3 = t1;
			t1 = t2;
			t2 = t3;
		}
        public static void Swap<T>(ref T t1, ref T t2, out T temp)
        {
            temp = t1;
            t1 = t2;
            t2 = t1;
        }
        public static IEnumerable<U> SelectIntersection<T, U>(this IEnumerable<T> array, Func<T, IEnumerable<U>> func)
        {
            int count = 0;
            Dictionary<U, int> temp = new Dictionary<U, int>();
            foreach (var list in array)
            {
                var result = func(list);
                foreach (var item in result)
                {
                    if (temp.ContainsKey(item))
                        temp[item] = temp[item] + 1;
                    else
                        temp[item] = 1;
                }
                count++;
            }
            foreach (var item in temp)
                if (item.Value == count)
                    yield return item.Key;
        }
        public static bool SameForAll<T>(this IEnumerable<T> array, Func<T, T, bool> func)
        {
            bool flag = false;
            T previous = default(T);
            foreach (var item in array)
            {
                if (flag)
                {
                    if (!func(previous, item))
                        return false;
                }
                else
                {
                    previous = item;
                    flag = true;
                }
            }
            return true;
        }


		#endregion


		#region Time


		public readonly static DateTime UtcTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public readonly static DateTime LocalTime = UtcTime + (DateTime.Now - DateTime.UtcNow);
        public const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

		public static long Timestamp
		{
			get { return ToTimestamp(DateTime.Now); }
		}
        /// <summary>2038年将超过int最大值</summary>
		public static int UnixTimestamp
		{
            get { return ToUnixTimestamp(DateTime.Now); }
		}

		public static long ToTimestamp(DateTime localTime)
		{
			return (long)(localTime - LocalTime).TotalMilliseconds;
		}
		public static DateTime ToTime(TimeSpan timestamp)
		{
			return LocalTime + timestamp;
		}
        public static DateTime ToTime(long milliseconds)
        {
            return LocalTime.AddMilliseconds(milliseconds);
        }
		public static int ToUnixTimestamp(DateTime localTime)
		{
            return (int)(localTime - LocalTime).TotalSeconds;
		}
		public static DateTime ToUnixTime(int timestamp)
		{
			return ToTime(TimeSpan.FromSeconds(timestamp));
		}


		#endregion


		#region 2 power enum


		public static bool EnumContains(int value, int target)
		{
			return (value & target) == target;
		}
		public static int EnumRemove(int value, int target)
		{
			return value & ~target;
		}
		public static bool EnumGetBool(int value, int index)
		{
			return (value & (1 << index)) != 0;
		}
		public static int EnumSetBool(int value, int index)
		{
			return value | (1 << index);
		}
		public static int EnumGetBoolIndex(int value)
		{
			int index = 0;
			while (value > 1)
			{
				index++;
				value = value >> 1;
			}
			return index;
		}
		public static int EnumGetBoolIndex(int previous, int current)
		{
			return EnumGetBoolIndex(previous ^ current);
		}
		public static int EnumHigh4(int value)
		{
			return (value & 0xf0) % 0x0f;
		}
		public static int EnumLow4(int value)
		{
			return value & 0x0f;
		}


		#endregion
    }
}
