using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace NetCode.Util
{
    public static partial class Extension
    {
        private delegate List<T> tableToListDelegate<T>(DataTable dataTable);

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, DynamicMethod> dynamicMethodCache = new ConcurrentDictionary<RuntimeTypeHandle, DynamicMethod>();

        /// <summary>
        /// DataTable 转 List 扩展方法
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="dataTable">数据表</param>
        /// <returns>目标类型列表</returns>
        public static List<T> EmitToListDynamic<T>(this DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count < 1)
                return new List<T>();

            var dynamicMethod = dynamicMethodCache.Value(typeof(T).TypeHandle, () =>
            {
                return Core.TableToList<T>();
            });

            return ((tableToListDelegate<T>)dynamicMethod.CreateDelegate(typeof(tableToListDelegate<T>)))(dataTable);
        }
    }

    public static class Core
    {
        /// <summary>
        /// 创建 DataTable 转 List 的方法
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>动态方法</returns>
        public static DynamicMethod TableToList<T>()
        {
            var instanceType = typeof(T);
            var instancesType = typeof(List<T>);

            DynamicMethod transMethod = new DynamicMethod(dynamicMethod + instanceType.Name, instancesType, new Type[] { typeof(DataTable) }, true);

            ILGenerator generator = transMethod.GetILGenerator();

            LocalBuilder instances = generator.DeclareLocal(instancesType); // List<T> 存储对象
            LocalBuilder totalCount = generator.DeclareLocal(typeof(int)); // Table 总记录数
            LocalBuilder currentRow = generator.DeclareLocal(typeof(DataRow)); // T 存储对象
            LocalBuilder currentRowIndex = generator.DeclareLocal(typeof(int)); // 当前Table.Rows的索引

            Label exit = generator.DefineLabel(); // 退出循环
            Label loop = generator.DefineLabel(); // 循环体

            generator.SetDefaultObjectValue(instancesType, instances);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Callvirt, Table.RowMethod);
            generator.Emit(OpCodes.Callvirt, Table.RowCountMethod);
            generator.Emit(OpCodes.Stloc_S, totalCount);

            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Stloc_S, currentRowIndex);

            generator.MarkLabel(loop); // 开始循环
            LocalBuilder instance = generator.DeclareLocal(instanceType);
            generator.SetDefaultObjectValue(instanceType, instance); // default(T);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Callvirt, Table.RowMethod);
            generator.Emit(OpCodes.Ldloc, currentRowIndex);
            generator.Emit(OpCodes.Callvirt, Table.RowItemMethod); // table.Rows[i]
            generator.Emit(OpCodes.Stloc_S, currentRow);

            generator.SetDataRowToEntityIL(instanceType, currentRow, instance);

            generator.Emit(OpCodes.Ldloc, instances);
            generator.Emit(OpCodes.Ldloc, instance);
            generator.Emit(OpCodes.Call, instancesType.GetMethod(insertEntity));

            // i++
            generator.Emit(OpCodes.Ldloc, currentRowIndex);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Add);
            generator.Emit(OpCodes.Stloc_S, currentRowIndex);
            // i < table.Rows.Count
            generator.Emit(OpCodes.Ldloc, currentRowIndex);
            generator.Emit(OpCodes.Ldloc, totalCount);
            generator.Emit(OpCodes.Clt);
            generator.Emit(OpCodes.Brtrue, loop);

            generator.Emit(OpCodes.Ldloc, instances);
            generator.Emit(OpCodes.Ret);

            return transMethod;
        }

        /// <summary>
        /// 根据类型创建默认的对象值
        /// </summary>
        /// <param name="generator">生产中间语言对象</param>
        /// <param name="instanceType">对象类型</param>
        /// <param name="instance">对象类型变量</param>
        public static void SetDefaultObjectValue(this ILGenerator generator, Type instanceType, LocalBuilder instance)
        {
            if (instanceType.IsClass)
            {
                generator.Emit(OpCodes.Newobj, instanceType.GetConstructor(Type.EmptyTypes));
                generator.Emit(OpCodes.Stloc_S, instance);
            }
            else
            {
                generator.Emit(OpCodes.Ldloca_S, instance);
                generator.Emit(OpCodes.Initobj, instanceType);
            }
        }

        /// <summary>
        /// 加载类型
        /// </summary>
        /// <param name="generator">生产中间语言对象</param>
        /// <param name="instanceType">对象类型</param>
        /// <param name="instance">对象类型变量</param>
        public static void LoadDefaultObject(this ILGenerator generator, Type instanceType, LocalBuilder instance)
        {
            if (instanceType.IsClass)
                generator.Emit(OpCodes.Ldloc, instance);
            else // structure 需要加载对象的地址
                generator.Emit(OpCodes.Ldloca, instance);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="generator">生产中间语言对象</param>
        /// <param name="instanceType">对象类型</param>
        /// <param name="method">方法</param>
        public static void SetObjectValue(this ILGenerator generator, Type instanceType, MethodInfo method)
        {
            if (instanceType.IsValueType)
                generator.Emit(OpCodes.Call, method);
            else
                generator.Emit(OpCodes.Callvirt, method);
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="generator">生产中间语言对象</param>
        /// <param name="propertyType">属性类型</param>
        public static void ConvertValue(this ILGenerator generator, Type propertyType)
        {
            if (propertyType.IsValueType)
                generator.Emit(OpCodes.Unbox_Any, propertyType); // 如果是值类型就拆箱
            else
                generator.Emit(OpCodes.Castclass, propertyType);
        }

        /// <summary>
        /// 设置 DataRow 转 Entity 中间语言
        /// </summary>
        /// <param name="generator">生产中间语言对象</param>
        /// <param name="instanceType">对象类型</param>
        /// <param name="currentRow">当前 DataRow</param>
        /// <param name="instance">对象类型变量</param>
        public static void SetDataRowToEntityIL(this ILGenerator generator, Type instanceType, LocalBuilder currentRow, LocalBuilder instance)
        {
            List<PropertyInfo> properties = Property.Properties(instanceType);
            foreach (var property in properties)
            {
                var setMethod = property.GetSetMethod(true);
                if (setMethod != null)
                {
                    if (TypeArray.Value.Contains(property.PropertyType) || property.PropertyType.IsEnum)
                    {
                        Label endIfLabel = generator.DefineLabel();

                        // 判断是否包含此属性,不区分大小写,不包含则直接调到结束位置
                        generator.Emit(OpCodes.Ldloc, currentRow);
                        generator.Emit(OpCodes.Ldstr, property.Name);
                        generator.Emit(OpCodes.Call, Table.ContainColumnMethod);
                        generator.Emit(OpCodes.Brfalse, endIfLabel);

                        // 获取此属性在 DataRow 中的值并赋值
                        LoadDefaultObject(generator, instanceType, instance);
                        generator.Emit(OpCodes.Ldloc, currentRow);
                        generator.Emit(OpCodes.Ldstr, property.Name);
                        generator.Emit(OpCodes.Call, Table.ColumnValueMethod);
                        // 值进行转换
                        ConvertValue(generator, property.PropertyType);
                        SetObjectValue(generator, instanceType, setMethod);

                        // 结束
                        generator.MarkLabel(endIfLabel);
                    }
                    else if (property.PropertyType.IsArray || property.PropertyType.IsGenericType) // 数组类型
                    {
                    }
                    else // 对象
                    {
                        LocalBuilder instanceProperty = generator.DeclareLocal(property.PropertyType);
                        SetDefaultObjectValue(generator, property.PropertyType, instanceProperty); // 实例对象
                        SetDataRowToEntityIL(generator, property.PropertyType, currentRow, instanceProperty);
                        LoadDefaultObject(generator, instanceType, instance);
                        generator.Emit(OpCodes.Ldloc, instanceProperty);
                        SetObjectValue(generator, instanceType, setMethod);
                    }
                }
            }
        }

        private const string insertEntity = "Add";
        private const string dynamicMethod = "Forge";
    }

    public static class Table
    {
        public static readonly MethodInfo RowMethod = typeof(DataTable).GetMethod("get_Rows", Type.EmptyTypes);

        public static readonly MethodInfo RowCountMethod = typeof(DataRowCollection).GetMethod("get_Count", Type.EmptyTypes);

        public static readonly MethodInfo RowItemMethod = typeof(DataRowCollection).GetMethod("get_Item", new Type[] { typeof(int) });

        public static readonly MethodInfo ColumnValueMethod = typeof(Table).GetMethod("ColumnValue", new Type[] { typeof(DataRow), typeof(string) });

        public static readonly MethodInfo ContainColumnMethod = typeof(Table).GetMethod("ContainColumn", new Type[] { typeof(DataRow), typeof(string) });

        /// <summary>
        /// 是否包含指定列名
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnName">列名</param>
        /// <returns>是否包含</returns>
        public static bool ContainColumn(DataRow row, string columnName)
        {
            if (row == null)
                return false;

            if (string.IsNullOrWhiteSpace(columnName))
                return false;

            return row.Table.Columns.IndexOf(columnName) > -1;
        }

        /// <summary>
        /// 获取指定列的值
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnName">列名</param>
        /// <returns>列值</returns>
        public static object ColumnValue(DataRow row, string columnName)
        {
            if (ContainColumn(row, columnName))
            {
                var value = row[columnName];
                return (value == null || value == DBNull.Value) ? null : value;
            }
            return null;
        }
    }

    public static class Property
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, List<PropertyInfo>> propertyCache = new ConcurrentDictionary<RuntimeTypeHandle, List<PropertyInfo>>();

        /// <summary>
        /// 获取类型的属性信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>属性信息列表</returns>
        public static List<PropertyInfo> Properties(Type type)
        {
            if (type == null)
                return new List<PropertyInfo>();

            return propertyCache.Value(type.TypeHandle, () =>
            {
                return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            });
        }
    }

    public static class ConcurrentDictionaryExtension
    {
        /// <summary>
        /// ConcurrentDictionary 获取值扩展方法
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="conDic">字典集合</param>
        /// <param name="key">键值</param>
        /// <param name="action">获取值方法(使用时需要注意方法闭包)</param>
        /// <returns>指定键的值</returns>
        public static TValue Value<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> concurrentDictionary, TKey key, Func<TValue> action)
        {
            TValue value;

            if (concurrentDictionary.TryGetValue(key, out value))
                return value;

            value = action();
            concurrentDictionary[key] = value;

            return value;
        }
    }

    public static class TypeArray
    {
        /// <summary>
        /// 类型数组
        /// </summary>
        public static readonly Type[] Value = new Type[] { typeof(int), typeof(int?), typeof(uint), typeof(uint?), typeof(short), typeof(short?), typeof(ushort), typeof(ushort?), typeof(long), typeof(long?), typeof(ulong), typeof(ulong?), typeof(byte), typeof(byte?), typeof(sbyte), typeof(sbyte?), typeof(bool), typeof(bool?), typeof(char), typeof(char?), typeof(float), typeof(float?), typeof(double), typeof(double?), typeof(decimal), typeof(decimal?), typeof(byte[]), typeof(string), typeof(object), typeof(Guid), typeof(Guid?), typeof(DateTime), typeof(DateTime?), typeof(TimeSpan), typeof(TimeSpan?), typeof(DateTimeOffset), typeof(DateTimeOffset?) };
    }
}