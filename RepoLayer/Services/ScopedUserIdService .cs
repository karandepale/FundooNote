using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Services
{
    public class ScopedUserIdService : IScopedUserIdService
    {
        public long UserId { get; set; }
    }
}
