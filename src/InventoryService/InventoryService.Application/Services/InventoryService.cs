using InventoryService.Application.DTOs;
using InventoryService.Domain.Abstractions;
using InventoryService.Domain.Models;
using MassTransit;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Application.Services;

public class InventoryAppService
{
    private readonly IInventoryRepository _repository;
    private readonly IPublishEndpoint _publisher;

    public InventoryAppService(IInventoryRepository repository, IPublishEndpoint publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<IEnumerable<InventoryItemDTO>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(x => new InventoryItemDTO
        {
            Id = x.Id,
            Name = x.Name,
            Quantity = x.Quantity
        });
    }

    public async Task AddItemAsync(string name, int quantity)
    {
        var item = new InventoryItem
        {
            Id = Guid.NewGuid(),
            Name = name,
            Quantity = quantity
        };
        await _repository.AddAsync(item);
    }

    public async Task<bool> DecreaseInventoryAsync(IList<OrderItemContractDTO> items)
    {
        var productIDs = items.Select(x => x.ProductId).ToList();
        var inventoryItems = await _repository.GetByIdRangeAsync(productIDs);
        if (inventoryItems == null) return false;

        foreach (var inventoryItem in inventoryItems)
        {
            var item = items.First(x => x.ProductId == inventoryItem.Id);
            inventoryItem.Decrease(item.Quantity);
            await _repository.UpdateAsync(inventoryItem);
        }

        var updatedItems = inventoryItems
            .Select(x => new OrderItemContractDTO
            {
                ProductId = x.Id,
                ProductName = x.Name,
                Quantity = x.Quantity
            }).ToList();

        await _publisher.Publish(new InventoryUpdated(updatedItems));

        return true;
    }
}

