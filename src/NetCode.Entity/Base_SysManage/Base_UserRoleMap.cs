using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCode.Entity.Base_SysManage
{
    /// <summary>
    /// Base_UserRoleMap
    /// </summary>
    [Table("Base_UserRoleMap")]
    public class Base_UserRoleMap
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String ID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        public String UserID { get; set; }

        /// <summary>
        /// RoleID
        /// </summary>
        public String RoleID { get; set; }

        /// <summary>
        /// CreateUser
        /// </summary>
        public String CreateUser { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}