using System.ComponentModel.DataAnnotations;
using Util.ApplicationServices;

namespace Applications.Services.Dtos.Systems
{
    public class ApplicationInTenantDto : ApplicationDto
    {
        [Display(Name = "选择")]
        public bool IsChecked { get; set; }
        public TenantDto Tenant { get; set; }
    }
}