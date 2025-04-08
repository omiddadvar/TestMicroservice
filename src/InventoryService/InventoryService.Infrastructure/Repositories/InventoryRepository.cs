using InventoryService.Domain.Abstractions;
using InventoryService.Domain.Models;
using InventoryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Infrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryContext _db;

    public InventoryRepository(InventoryContext db)
    {
        _db = db;
    }

    public async Task AddAsync(InventoryItem item)
    {
        _db.InventoryItems.Add(item);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<InventoryItem>> GetAllAsync() =>
        await _db.InventoryItems.ToListAsync();

    public async Task<InventoryItem?> GetByIdAsync(Guid id) =>
        await _db.InventoryItems.FindAsync(id);

    public async Task<IList<InventoryItem>?> GetByIdRangeAsync(IList<Guid> items)
    
        => await _db.InventoryItems.Where(x => items.Contains(x.Id)).ToListAsync();
    

    public async Task UpdateAsync(InventoryItem item)
    {
        _db.InventoryItems.Update(item);
        await _db.SaveChangesAsync();
    }
}

