using System.Collections.Generic;

namespace NetCode.Util
{
    /// <summary>
    /// 树模型（可以作为父类）
    /// </summary>
    public class TreeModel
    {
        /// <summary>
        /// 唯一标识Id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderNo { get; set; }

        /// <summary>
        /// 节点深度 控制显示缩进
        /// </summary>
        public int? Level { get; set; } = 1;

        /// <summary>
        /// 孩子节点
        /// </summary>
        public List<object> Children { get; set; }
    }
}
