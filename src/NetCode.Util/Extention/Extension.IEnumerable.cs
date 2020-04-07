using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;

namespace NetCode.Util
{
    public static partial class Extension
    {
        /// <summary>
        /// 数据替换
        /// 注：支持一次性替换多个，支持所有可迭代类型，KeyValuePair键值对中Key为需要替换的数据，Value为替换后数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="parttern">替换模式</param>
        /// <returns></returns>
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, params KeyValuePair<IEnumerable<T>, IEnumerable<T>>[] parttern)
        {
            List<T> resList = new List<T>();
            List<T> sourceList = source.ToList();
            for (int i = 0; i < sourceList.Count; i++)
            {
                bool replaced = false;
                parttern.ForEach(aMatch =>
                {
                    var oldvalue = aMatch.Key.ToList();
                    var newvalue = aMatch.Value.ToList();

                    bool needReplace = true;
                    for (int j = 0; j < oldvalue.Count; j++)
                    {
                        if (!sourceList[i + j].Equals(oldvalue[j]))
                            needReplace = false;
                    }
                    if (needReplace)
                    {
                        resList.AddRange(newvalue);
                        i = i + oldvalue.Count - 1;
                        replaced = true;
                    }
                });
                if (!replaced)
                    resList.Add(sourceList[i]);
            }

            return resList;
        }

        /// <summary>
        /// 复制序列中的数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="iEnumberable">原数据</param>
        /// <param name="startIndex">原数据开始复制的起始位置</param>
        /// <param name="length">需要复制的数据长度</param>
        /// <returns></returns>
        public static IEnumerable<T> Copy<T>(this IEnumerable<T> iEnumberable, int startIndex, int length)
        {
            var sourceArray = iEnumberable.ToArray();
            T[] newArray = new T[length];
            Array.Copy(sourceArray, startIndex, newArray, 0, length);

            return newArray;
        }

        /// <summary>
        /// 序列连接对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="iEnumberable">原序列</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> iEnumberable, T obj)
        {
            return iEnumberable.Concat(new T[] { obj });
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T> func)
        {
            foreach (var item in iEnumberable)
            {
                func(item);
            }
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T, int> func)
        {
            var array = iEnumberable.ToArray();
            for (int i = 0; i < array.Count(); i++)
            {
                func(array[i], i);
            }
        }

        /// <summary>
        /// IEnumerable转换为List'T'
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static List<T> CastToList<T>(this IEnumerable source)
        {
            return new List<T>(source.Cast<T>());
        }

        /// <summary>
        /// 将IEnumerable'T'转为对应的DataTable
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTableByJson<T>(this IEnumerable<T> iEnumberable)
        {
            return iEnumberable.ToJson().ToDataTable();
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public static IEnumerable<T> GetPagination<T>(this IEnumerable<T> iEnumberable, Pagination pagination)
        {
            pagination.records = iEnumberable.Count();
            
            return iEnumberable.OrderBy($@"{pagination.SortField} {pagination.SortType}").Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows).ToList();
        }

        /// <summary>
        /// 判断集合是否有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this IEnumerable<T> source)
        {
            return (source != null && source.Count() != 0)
                ? true : false;
        }

        /// <summary>
        /// IEnumerable转换为DataTable
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                var name = prop.Name;
                var att = prop.GetCustomAttribute<DisplayNameAttribute>();
                if (att != null && att.DispalyName != string.Empty)
                {
                    name = att.DispalyName;
                }
                dataTable.Columns.Add(name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                    prop.PropertyType);
            }

            foreach (T item in source)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        /// <summary>
        /// 去重
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// 获取链表针对处理器数*2 所划分的条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ReserveN<T>(this IEnumerable<T> source)
        {
            var processCount = Environment.ProcessorCount;
            return source.Count() / (processCount * 2);
        }

        /// <summary>
        /// 判断链表非空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNotNull<T>(this IEnumerable<T> source)
        {
            return null != source && 0 < source.Count() ? true : false;
        }

        /// <summary>
        /// 转换为（'',''）格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wheres"></param>
        /// <returns></returns>
        public static string TryToWhere<T>(this IEnumerable<T> wheres)
        {
            return new
                   StringBuilder("('" + string.Join("','", wheres) + "')").ToString();
        }

        /// <summary>
        /// 生成SQL中IN的条件集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public static List<string> TryToBatchValue<T>(this List<T> bucket)
        {
            var proce = bucket.ReserveN();
            var batch = bucket.Count / proce;
            var mores = bucket.Count % proce;
            var sizes = mores == 0
                      ? batch : batch + 1;

            var temps = new List<string>();

            for (int i = 0; i < sizes; i++)
            {
                if (i == sizes - 1 && bucket.Take(mores).IsNotNull())
                {
                    temps.Add(bucket.Take(mores).TryToWhere());
                }
                else
                {
                    temps.Add(bucket.Take(proce).TryToWhere());
                    bucket.RemoveRange(0, proce);
                }
            }
            return temps;
        }
    }

    /// <summary>
    /// 包含数据库表字段名称的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DisplayNameAttribute : Attribute
    {
        public string DispalyName { get; set; }
        public DisplayNameAttribute(string DispalyName)
        {

            this.DispalyName = DispalyName;
        }
    }
}
