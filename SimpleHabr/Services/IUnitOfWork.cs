using System;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public interface IUnitOfWork
    {
        IPostService Posts { get; }
        IAuthService Users { get; }
        IGenericService<Comment> Comments {get;}
        IGenericService<Like> Likes { get; }

    }
}
