#if dotnet2
using System.Collections.Generic;
#endif
using System.Collections;

namespace IBatisNet.DataMapper.Test.Domain
{
    public class Application
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Role role;
        public Role DefaultRole
        {
            get { return role; }
            set { role = value; }
        }

#if dotnet2
        private IList<ApplicationUser> users;
        public IList<ApplicationUser> Users
        {
            get { return users; }
            set { users = value; }
        }
#else
        private IList users;
        public IList Users
        {
            get { return users; }
            set { roles = users; }
        }        
#endif
    }
}
