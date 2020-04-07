namespace NetCode.Util
{
    /// <summary>
    /// 数据库表的信息
    /// </summary>
    public class DbTableInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表描述说明
        /// </summary>
        public string Description
        {
            get
            {
                return _description.IsNullOrEmpty() ? TableName : _description;
            }
            set
            {
                _description = value;
            }
        }

        private string _description { get; set; }
    }

    /// <summary>
    /// 数据库表字段的信息
    /// </summary>
    public class DbFieldInfo
    {
        /// <summary>
        /// 字段Id
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsKey { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 字段描述说明
        /// </summary>
        public string Description
        {
            get
            {
                return _description.IsNullOrEmpty() ? Name : _description;
            }
            set
            {
                _description = value;
            }
        }

        private string _description { get; set; }
    }
}
