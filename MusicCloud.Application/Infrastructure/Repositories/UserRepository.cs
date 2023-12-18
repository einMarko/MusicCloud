using MusicCloud.Application.infrastructure;
using MusicCloud.Application.Model;
using MusicCloud.Application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCloud.Application.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, int>
    {
        private readonly ICryptService _cryptService;
        public UserRepository(CloudContext db, ICryptService cryptService) : base(db) 
        {
            _cryptService = cryptService;
        }

    }
}
