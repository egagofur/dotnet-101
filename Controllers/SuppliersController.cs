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
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierResponse>>> GetAll()
    {
        var result = await _supplierService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierResponse>> GetById(Guid id)
    {
        var result = await _supplierService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierResponse>> Create([FromBody] CreateSupplierRequest request)
    {
        var result = await _supplierService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SupplierResponse>> Update(Guid id, [FromBody] UpdateSupplierRequest request)
    {
        var result = await _supplierService.UpdateAsync(id, request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _supplierService.DeleteAsync(id);
        return NoContent();
    }
}
