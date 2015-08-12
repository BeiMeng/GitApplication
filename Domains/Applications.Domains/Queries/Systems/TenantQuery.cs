using System;
using System.ComponentModel.DataAnnotations;
using Applications.Domains.Enums;
using Util;
using Util.Domains;
using Util.Domains.Repositories;

namespace Applications.Domains.Queries.Systems {
    /// <summary>
    /// 租户查询实体
    /// </summary>
    public class TenantQuery : TreeEntityQuery
    {
        /// <summary>
        /// 租户编号
        /// </summary>
        [Display( Name = "租户编号" )]
        public Guid? TenantId { get; set; }

        private string _code = string.Empty;
        /// <summary>
        /// 租户编码
        /// </summary>
        [Display( Name = "租户编码" )]
        public string Code {
            get { return _code == null ? string.Empty : _code.Trim(); }
            set { _code = value; }
        }

        private string _name = string.Empty;
        /// <summary>
        /// 租户名称
        /// </summary>
        [Display( Name = "租户名称" )]
        public string Name {
            get { return _name == null ? string.Empty : _name.Trim(); }
            set { _name = value; }
        }

        /// <summary>
        /// 父编号
        /// </summary>
        [Display( Name = "父编号" )]
        public Guid? ParentId { get; set; }

        private string _path = string.Empty;
        /// <summary>
        /// 路径
        /// </summary>
        [Display( Name = "路径" )]
        public string Path {
            get { return _path == null ? string.Empty : _path.Trim(); }
            set { _path = value; }
        }


        private string _contactName = string.Empty;
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [Display( Name = "联系人姓名" )]
        public string ContactName {
            get { return _contactName == null ? string.Empty : _contactName.Trim(); }
            set { _contactName = value; }
        }

        private string _email = string.Empty;
        /// <summary>
        /// 电子邮件
        /// </summary>
        [Display( Name = "电子邮件" )]
        public string Email {
            get { return _email == null ? string.Empty : _email.Trim(); }
            set { _email = value; }
        }

        private string _phone = string.Empty;
        /// <summary>
        /// 电话
        /// </summary>
        [Display( Name = "电话" )]
        public string Phone {
            get { return _phone == null ? string.Empty : _phone.Trim(); }
            set { _phone = value; }
        }

        private string _mobilePhone = string.Empty;
        /// <summary>
        /// 手机
        /// </summary>
        [Display( Name = "手机" )]
        public string MobilePhone {
            get { return _mobilePhone == null ? string.Empty : _mobilePhone.Trim(); }
            set { _mobilePhone = value; }
        }

        private string _fax = string.Empty;
        /// <summary>
        /// 传真
        /// </summary>
        [Display( Name = "传真" )]
        public string Fax {
            get { return _fax == null ? string.Empty : _fax.Trim(); }
            set { _fax = value; }
        }

        private string _qq = string.Empty;
        /// <summary>
        /// Qq
        /// </summary>
        [Display( Name = "Qq" )]
        public string Qq {
            get { return _qq == null ? string.Empty : _qq.Trim(); }
            set { _qq = value; }
        }

        /// <summary>
        /// 省份编号
        /// </summary>
        [Display( Name = "省份编号" )]
        public Guid? ProvinceId { get; set; }

        private string _province = string.Empty;
        /// <summary>
        /// 省份
        /// </summary>
        [Display( Name = "省份" )]
        public string Province {
            get { return _province == null ? string.Empty : _province.Trim(); }
            set { _province = value; }
        }

        /// <summary>
        /// 城市编号
        /// </summary>
        [Display( Name = "城市编号" )]
        public Guid? CityId { get; set; }

        private string _city = string.Empty;
        /// <summary>
        /// 城市
        /// </summary>
        [Display( Name = "城市" )]
        public string City {
            get { return _city == null ? string.Empty : _city.Trim(); }
            set { _city = value; }
        }

        /// <summary>
        /// 区县编号
        /// </summary>
        [Display( Name = "区县编号" )]
        public Guid? CountyId { get; set; }

        private string _county = string.Empty;
        /// <summary>
        /// 区县
        /// </summary>
        [Display( Name = "区县" )]
        public string County {
            get { return _county == null ? string.Empty : _county.Trim(); }
            set { _county = value; }
        }

        private string _street = string.Empty;
        /// <summary>
        /// 街道
        /// </summary>
        [Display( Name = "街道" )]
        public string Street {
            get { return _street == null ? string.Empty : _street.Trim(); }
            set { _street = value; }
        }

        private string _zip = string.Empty;
        /// <summary>
        /// 邮政编码
        /// </summary>
        [Display( Name = "邮政编码" )]
        public string Zip {
            get { return _zip == null ? string.Empty : _zip.Trim(); }
            set { _zip = value; }
        }

        private string _siteName = string.Empty;
        /// <summary>
        /// 站点名称
        /// </summary>
        [Display( Name = "站点名称" )]
        public string SiteName {
            get { return _siteName == null ? string.Empty : _siteName.Trim(); }
            set { _siteName = value; }
        }

        private string _siteTitle = string.Empty;
        /// <summary>
        /// 站点标题
        /// </summary>
        [Display( Name = "站点标题" )]
        public string SiteTitle {
            get { return _siteTitle == null ? string.Empty : _siteTitle.Trim(); }
            set { _siteTitle = value; }
        }

        private string _siteDescription = string.Empty;
        /// <summary>
        /// 站点描述
        /// </summary>
        [Display( Name = "站点描述" )]
        public string SiteDescription {
            get { return _siteDescription == null ? string.Empty : _siteDescription.Trim(); }
            set { _siteDescription = value; }
        }

        private string _keyWords = string.Empty;
        /// <summary>
        /// 搜索关键字
        /// </summary>
        [Display( Name = "搜索关键字" )]
        public string KeyWords {
            get { return _keyWords == null ? string.Empty : _keyWords.Trim(); }
            set { _keyWords = value; }
        }

        private string _statCode = string.Empty;
        /// <summary>
        /// 流量统计代码
        /// </summary>
        [Display( Name = "流量统计代码" )]
        public string StatCode {
            get { return _statCode == null ? string.Empty : _statCode.Trim(); }
            set { _statCode = value; }
        }

        private string _icp = string.Empty;
        /// <summary>
        /// ICP备案号
        /// </summary>
        [Display( Name = "ICP备案号" )]
        public string Icp {
            get { return _icp == null ? string.Empty : _icp.Trim(); }
            set { _icp = value; }
        }

        private string _copyright = string.Empty;
        /// <summary>
        /// 版权信息
        /// </summary>
        [Display( Name = "版权信息" )]
        public string Copyright {
            get { return _copyright == null ? string.Empty : _copyright.Trim(); }
            set { _copyright = value; }
        }

        private string _urls = string.Empty;
        /// <summary>
        /// 站点域名
        /// </summary>
        [Display( Name = "站点域名" )]
        public string Urls {
            get { return _urls == null ? string.Empty : _urls.Trim(); }
            set { _urls = value; }
        }

        private string _skin = string.Empty;
        /// <summary>
        /// 站点皮肤
        /// </summary>
        [Display( Name = "站点皮肤" )]
        public string Skin {
            get { return _skin == null ? string.Empty : _skin.Trim(); }
            set { _skin = value; }
        }

        private string _menuStyle = string.Empty;
        /// <summary>
        /// 导航菜单样式
        /// </summary>
        [Display( Name = "导航菜单样式" )]
        public string MenuStyle {
            get { return _menuStyle == null ? string.Empty : _menuStyle.Trim(); }
            set { _menuStyle = value; }
        }

        /// <summary>
        /// 允许登录
        /// </summary>
        [Display( Name = "允许登录" )]
        public bool? AllowLogin { get; set; }

        /// <summary>
        /// 允许注册
        /// </summary>
        [Display( Name = "允许注册" )]
        public bool? AllowRegister { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [Display( Name = "启用" )]
        public bool? Enabled { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Display( Name = "状态" )]
        public TenantState? TState { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display( Name = "排序号" )]
        public int? SortId { get; set; }

        private string _pinYin = string.Empty;
        /// <summary>
        /// 拼音简码
        /// </summary>
        [Display( Name = "拼音简码" )]
        public string PinYin {
            get { return _pinYin == null ? string.Empty : _pinYin.Trim(); }
            set { _pinYin = value; }
        }

        private string _note = string.Empty;
        /// <summary>
        /// 备注
        /// </summary>
        [Display( Name = "备注" )]
        public string Note {
            get { return _note == null ? string.Empty : _note.Trim(); }
            set { _note = value; }
        }

        /// <summary>
        /// 起始创建时间
        /// </summary>
        [Display( Name = "起始创建时间" )]
        public DateTime? BeginCreateTime { get; set; }

        /// <summary>
        /// 结束创建时间
        /// </summary>
        [Display( Name = "结束创建时间" )]
        public DateTime? EndCreateTime { get; set; }

        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
            base.AddDescriptions();
            AddDescription( "租户编号", TenantId );
            AddDescription( "租户编码", Code );
            AddDescription( "租户名称", Name );
            AddDescription( "父编号", ParentId );
            AddDescription( "路径", Path );
            AddDescription( "联系人姓名", ContactName );
            AddDescription( "电子邮件", Email );
            AddDescription( "电话", Phone );
            AddDescription( "手机", MobilePhone );
            AddDescription( "传真", Fax );
            AddDescription( "Qq", Qq );
            AddDescription( "省份编号", ProvinceId );
            AddDescription( "省份", Province );
            AddDescription( "城市编号", CityId );
            AddDescription( "城市", City );
            AddDescription( "区县编号", CountyId );
            AddDescription( "区县", County );
            AddDescription( "街道", Street );
            AddDescription( "邮政编码", Zip );
            AddDescription( "站点名称", SiteName );
            AddDescription( "站点标题", SiteTitle );
            AddDescription( "站点描述", SiteDescription );
            AddDescription( "搜索关键字", KeyWords );
            AddDescription( "流量统计代码", StatCode );
            AddDescription( "ICP备案号", Icp );
            AddDescription( "版权信息", Copyright );
            AddDescription( "站点域名", Urls );
            AddDescription( "站点皮肤", Skin );
            AddDescription( "导航菜单样式", MenuStyle );
            AddDescription( "允许登录", AllowLogin.Description() );
            AddDescription( "允许注册", AllowRegister.Description() );
            AddDescription( "启用", Enabled.Description() );
            AddDescription( "状态", TState.Description() );
            AddDescription( "排序号", SortId );
            AddDescription( "拼音简码", PinYin );
            AddDescription( "备注", Note );
            AddDescription( "起始创建时间", BeginCreateTime );
            AddDescription( "结束创建时间", EndCreateTime );
        }
    }
}
