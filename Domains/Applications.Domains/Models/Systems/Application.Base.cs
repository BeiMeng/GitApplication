using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Util.Domains;

namespace Applications.Domains.Models.Systems
{
    /// <summary>
    /// 应用程序
    /// </summary>
    [DisplayName("应用程序")]
    public partial class Application : AggregateRoot<Application>
    {
        public Application() : base(Guid.Empty)
        {
        }
        public Application(Guid id) : base(id)
        {
            Tenants=new List<Tenant>();
        }

        /// <summary>
        /// 应用程序编码
        /// </summary>
        [Required(ErrorMessage = "应用程序编码不能为空")]
        [StringLength(10, ErrorMessage = "应用程序编码输入过长，不能超过10位")]
        public string Code { get; set; }
        /// <summary>
        /// 应用程序名称
        /// </summary>
        [Required(ErrorMessage = "应用程序名称不能为空")]
        [StringLength(30, ErrorMessage = "应用程序名称输入过长，不能超过30位")]
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100, ErrorMessage = "备注输入过长，不能超过100位")]
        public string Note { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        [Required(ErrorMessage = "启用不能为空")]
        public bool Enabled { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required(ErrorMessage = "创建时间不能为空")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 租户列表
        /// </summary>
        public virtual ICollection<Tenant> Tenants { get; set; }

        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions()
        {
            AddDescription("应用程序编号", Id);
            AddDescription("应用程序编码", Code);
            AddDescription("应用程序名称", Name);
            AddDescription("备注", Note);
            AddDescription("启用", Enabled.Description());
            AddDescription("创建时间", CreateTime);
        }

        /// <summary>
        /// 添加变更列表
        /// </summary>
        protected override void AddChanges(Application newEntity)
        {
            AddChange("Id", "应用程序编号", Id, newEntity.Id);
            AddChange("Code", "应用程序编码", Code, newEntity.Code);
            AddChange("Name", "应用程序名称", Name, newEntity.Name);
            AddChange("Note", "备注", Note, newEntity.Note);
            AddChange("Enabled", "启用", Enabled, newEntity.Enabled);
            AddChange("CreateTime", "创建时间", CreateTime, newEntity.CreateTime);
            AddChange("Version", "版本号", Version, newEntity.Version);
        }
    }
}
