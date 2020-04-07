using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCode.Entity.Base_SysManage
{
    /// <summary>
    /// Base_User
    /// </summary>
    [Table("Base_User")]
    public class Base_User
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String ID { get; set; }

        /// <summary>
        /// LoginName
        /// </summary>
        public String LoginName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// IdentityCard
        /// </summary>
        public String IdentityCard { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// IsDelete
        /// </summary>
        public Boolean? IsDelete { get; set; }

        /// <summary>
        /// 1 ∆Ù”√
        /// 0 Œ¥∆Ù”√
        /// </summary>
        public Boolean? IsValid { get; set; }

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