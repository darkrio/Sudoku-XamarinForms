using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Riots.Standard
{
    public class Model
    {
        /// <summary>
        ///  数值返回值
        /// </summary>
        public class CountResultJson
        {
            private int _count;
            public int Result { get => _count; set => _count = value; }
            public CountResultJson(int context)
            {
                _count = context;
            }
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        /// <summary>
        ///  数值返回列表
        /// </summary>
        public class CountResultListJson : StandardJsonList<CountResultJson> { }

        /// <summary>
        ///  字符串返回值
        /// </summary>
        public class MessageResultJson
        {
            private string errorCode = string.Empty;
            private string result = string.Empty;
            public string ErrorCode { get => errorCode; set => errorCode = value; }
            public string Result { get => result; set => result = value; }
            /// <summary>
            /// </summary>
            /// <param name="context"></param>
            public MessageResultJson(string context)
            {
                Result = context.Trim();
            }
            /// <summary>
            /// </summary>
            /// <param name="errorCode"></param>
            /// <param name="context"></param>
            public MessageResultJson(string errorCode, string context)
            {
                ErrorCode = errorCode.Trim();
                Result = context.Trim();
            }
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        /// <summary>
        /// 字符串返回列表
        /// </summary>
        public class MessageResultListJson : StandardJsonList<MessageResultJson> { }

        /// <summary>
        ///  键值对值
        /// </summary>
        public class PairResultJson
        {
            private string key = string.Empty;
            private string value = string.Empty;
            public string Key { get => key; set => key = value; }
            public string Value { get => value; set => this.value = value; }
            public PairResultJson(string key, string value)
            {
                Key = key;
                Value = value;
            }
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        /// <summary>
        /// 键值对列表
        /// </summary>
        public class PairResultListJson : StandardJsonList<PairResultJson> { }

        /// <summary>
        /// 标准列表返回类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class StandardJsonList<T> : IList<T>
        {
            List<T> _items = new List<T>();

            public List<T> Content
            {
                get { return _items; }
                set { _items = value; }
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(_items.ToArray());
            }

            public DataTable ToDataTable()
            {
                return _items.ToDataTable();
            }

            public int Count => ((ICollection<T>)_items).Count;

            public bool IsReadOnly => ((ICollection<T>)_items).IsReadOnly;

            public T this[int index] { get => ((IList<T>)_items)[index]; set => ((IList<T>)_items)[index] = value; }

            public int IndexOf(T item)
            {
                return ((IList<T>)_items).IndexOf(item);
            }

            public void Insert(int index, T item)
            {
                ((IList<T>)_items).Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                ((IList<T>)_items).RemoveAt(index);
            }

            public void Add(T item)
            {
                ((ICollection<T>)_items).Add(item);
            }

            public void Clear()
            {
                ((ICollection<T>)_items).Clear();
            }

            public bool Contains(T item)
            {
                return ((ICollection<T>)_items).Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                ((ICollection<T>)_items).CopyTo(array, arrayIndex);
            }

            public bool Remove(T item)
            {
                return ((ICollection<T>)_items).Remove(item);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return ((IEnumerable<T>)_items).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)_items).GetEnumerator();
            }
        }

        /// <summary>
        /// 标准值分页类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class StandardObjectAsPage<T>
        {
            /// <summary>
            ///     错误信息
            /// </summary>
            public string Error;

            /// <summary>
            ///     数据内容
            /// </summary>
            public T Items;

            /// <summary>
            ///     页面总数，共几页
            /// </summary>
            public int PageCount;

            /// <summary>
            ///     页面序号，第几页
            /// </summary>
            public int PageIndex = 1;

            /// <summary>
            ///     页面大小。每页显示多少项目
            /// </summary>
            public int PageSize;

            /// <summary>
            ///     状态
            /// </summary>
            public int Status;

            /// <summary>
            ///     数据总数
            /// </summary>
            public int Total;
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}
