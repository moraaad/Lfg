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

namespace LFG.Ordini;

public class EfCoreOrdineRepository : EfCoreOrdineRepositoryBase, IOrdineRepository
{
    public EfCoreOrdineRepository(IDbContextProvider<LFGDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}