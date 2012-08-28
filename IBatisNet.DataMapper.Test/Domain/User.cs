#if dotnet2
using System.Collections.Generic;
#endif
using System.Collections;

namespace IBatisNet.DataMapper.Test.Domain
{
    public class ApplicationUser
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;
        public string UserName
        {
            get { return name; }
            set { name = value; }
        }

        private Address address;
        public Address Address
        {
            get { return address; }
            set { address = value; }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

#if dotnet2
        private IList<Role> roles;
        public IList<Role> Roles
        {
            get { return roles; }
            set { roles = value; }
        }
#else
        private IList roles;
        public IList Roles
        {
            get { return roles; }
            set { roles = value; }
        }        
#endif
    }
}
