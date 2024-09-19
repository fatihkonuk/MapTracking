using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services.Implementations;

namespace WebApi.Services.Interfaces
{
    public interface IUnitOfWork
    {
        FeatureRepository FeatureRepository { get; }
        UserRepository UserRepository { get; }
        Task CompleteAsync();
    }
}