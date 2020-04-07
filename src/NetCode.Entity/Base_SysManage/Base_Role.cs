using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCode.Entity.Base_SysManage
{
    /// <summary>
    /// Base_Role
    /// </summary>
    [Table("Base_Role")]
    public class Base_Role
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String ID { get; set; }

        /// <summary>
        /// RoleName
        /// </summary>
        public String RoleName { get; set; }

        /// <summary>
        /// RoleDes
        /// </summary>
        public String RoleDes { get; set; }

        /// <summary>
        /// CreateUser
        /// </summary>
        public String CreateUser { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// UpdateUser
        /// </summary>
        public String UpdateUser { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime? UpdateTime { get; set; }

    }
}