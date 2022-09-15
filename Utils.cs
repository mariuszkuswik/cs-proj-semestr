using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace cs_proj_ostateczny
{
    public static class Utils
    {
        public static int getNextId(DbSet db)
        {
            var current_id = 0;
            foreach (var entity in db)
            {
                IdClass e = (IdClass)entity;

                if (e.id > current_id)
                {
                    current_id = e.id;
                }
            }

            return current_id + 1;
        }
    }
}
