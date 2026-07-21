using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WarehouseApi.DTOs;
using WarehouseApi.Services.Interface;

namespace WarehouseApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MovementsController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public MovementsController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockMovementResponse>>> GetAll()
    {
        var result = await _inventoryService.GetAllMovementsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StockMovementResponse>> GetById(Guid id)
    {
        var result = await _inventoryService.GetMovementByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<StockMovementResponse>> Create([FromBody] StockMovementRequest request)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "unknown@wms.com";

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedAccessException("User ID tidak valid dalam token.");
        }

        var result = await _inventoryService.CreateMovementAsync(userId, userEmail, request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
