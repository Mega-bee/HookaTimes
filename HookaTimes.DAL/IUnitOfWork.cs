using HookaTimes.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.DAL
{
    public interface IUnitOfWork
    {
        IProfileRepos ProfileRepos { get; }


        void Save();
    }
}
