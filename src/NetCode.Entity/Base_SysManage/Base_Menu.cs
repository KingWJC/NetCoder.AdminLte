using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCode.Entity.Base_SysManage
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Table("Base_Menu")]
    public class Base_Menu
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String ID { get; set; }

        /// <summary>
        /// MenuCode
        /// </summary>
        public String MenuCode { get; set; }

        /// <summary>
        /// MenuName
        /// </summary>
        public String MenuName { get; set; }

        /// <summary>
        /// ParentMenuCode
        /// </summary>
        public String ParentID { get; set; }

        /// <summary>
        /// 1.文件夹类型
        /// 2.页面
        /// 3.内容页面
        /// 4.操作
        /// </summary>
        public Int32? MenuType { get; set; }

        /// <summary>
        /// OrderNo
        /// </summary>
        public Int32? OrderNo { get; set; }

        /// <summary>
        /// PageUrl
        /// </summary>
        public String PageUrl { get; set; }

        /// <summary>
        /// PageUrlOriginal
        /// </summary>
        public String PageUrlOriginal { get; set; }

        /// <summary>
        /// IsAccessControl
        /// </summary>
        public Boolean? IsAccessControl { get; set; }

        /// <summary>
        /// IsValid
        /// </summary>
        public Boolean? IsValid { get; set; }

        /// <summary>
        /// IsLeaf
        /// </summary>
        public Boolean? IsLeaf { get; set; }

        /// <summary>
        /// SysCode
        /// </summary>
        public String SysCode { get; set; }
    }
}