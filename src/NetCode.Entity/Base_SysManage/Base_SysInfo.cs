using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCode.Entity.Base_SysManage
{
    /// <summary>
    /// Base_SysInfo
    /// </summary>
    [Table("Base_SysInfo")]
    public class Base_SysInfo
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String ID { get; set; }

        /// <summary>
        /// SysCode
        /// </summary>
        public String SysCode { get; set; }

        /// <summary>
        /// SysName
        /// </summary>
        public String SysName { get; set; }

        /// <summary>
        /// OrderNo
        /// </summary>
        public Int32? OrderNo { get; set; }

        /// <summary>
        /// IconUrl
        /// </summary>
        public String IconUrl { get; set; }

        /// <summary>
        /// IsValid
        /// </summary>
        public Boolean? IsValid { get; set; }

        /// <summary>
        /// IsAccessControl
        /// </summary>
        public Boolean? IsAccessControl { get; set; }

    }
}