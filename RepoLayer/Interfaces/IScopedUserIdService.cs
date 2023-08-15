using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interfaces
{
    public interface IScopedUserIdService
    {
        long UserId { get; set; }
    }
}
