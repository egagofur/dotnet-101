using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.DTOs;
using WarehouseApi.Services.Interface;

namespace WarehouseApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WarehouseResponse>>> GetAll()
    {
        var result = await _warehouseService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WarehouseResponse>> GetById(Guid id)
    {
        var result = await _warehouseService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WarehouseResponse>> Create([FromBody] CreateWarehouseRequest request)
    {
        var result = await _warehouseService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WarehouseResponse>> Update(Guid id, [FromBody] UpdateWarehouseRequest request)
    {
        var result = await _warehouseService.UpdateAsync(id, request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _warehouseService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/locations")]
    public async Task<ActionResult<LocationResponse>> AddLocation(Guid id, [FromBody] CreateLocationRequest request)
    {
        var result = await _warehouseService.AddLocationAsync(id, request);
        return Ok(result);
    }
}
