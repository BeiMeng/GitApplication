using System.Collections.Generic;
using Util.Security;

namespace Infrastructure.Security {
    public class SecurityManager : ISecurityManager{
        public bool IsInApplication( string userId ) {
            return true;
        }

        public bool IsInTenant( string userId ) {
            return true;
        }

        public ResourcePermissions GetPermissionsByResource( string resourceUri ) {
            return new ResourcePermissions( resourceUri, new List<Permission>() );
        }
    }
}
