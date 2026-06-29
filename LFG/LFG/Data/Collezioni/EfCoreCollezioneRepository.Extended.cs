using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using LFG.Data;

namespace LFG.Collezioni;

public class EfCoreCollezioneRepository : EfCoreCollezioneRepositoryBase, ICollezioneRepository
{
    public EfCoreCollezioneRepository(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}