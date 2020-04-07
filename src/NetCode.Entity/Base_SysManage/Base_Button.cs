using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCode.Entity.Base_SysManage
{
    /// <summary>
    /// Base_Button
    /// </summary>
    [Table("Base_Button")]
    public class Base_Button
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String ID { get; set; }

        /// <summary>
        /// BtnName
        /// </summary>
        public String BtnName { get; set; }

        /// <summary>
        /// BtnCode
        /// </summary>
        public String BtnCode { get; set; }

        /// <summary>
        /// BtnClass
        /// </summary>
        public String BtnClass { get; set; }

        /// <summary>
        /// BtnIcon
        /// </summary>
        public String BtnIcon { get; set; }

        /// <summary>
        /// BtnScript
        /// </summary>
        public String BtnScript { get; set; }

        /// <summary>
        /// MenuID
        /// </summary>
        public String MenuID { get; set; }

        /// <summary>
        /// InitStatus
        /// </summary>
        public String InitStatus { get; set; }

        /// <summary>
        /// SeqNo
        /// </summary>
        public Int32? SeqNo { get; set; }

    }
}