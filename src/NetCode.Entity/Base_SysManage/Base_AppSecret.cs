using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCode.Entity.Base_SysManage
{
    /// <summary>
    /// Ӧ����Կ��
    /// </summary>
    [Table("Base_AppSecret")]
    public class Base_AppSecret
    {

        /// <summary>
        /// ��Ȼ����
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// Ӧ��Id
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// Ӧ����Կ
        /// </summary>
        public String AppSecret { get; set; }

        /// <summary>
        /// Ӧ����
        /// </summary>
        public String AppName { get; set; }

    }
}