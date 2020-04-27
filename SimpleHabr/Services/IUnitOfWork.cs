using System;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public interface IUnitOfWork
    {
        IGenericService<Post> Posts { get; }
        IAuthService Users { get; }


    }
}
