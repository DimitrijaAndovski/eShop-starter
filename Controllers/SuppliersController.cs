using eShop.DTOs.Suppliers;
using eShop.Entities;
using eShop.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.Controllers;

[Route("api/suppliers")]
[ApiController]
public class SuppliersController(IGenericRepository<Supplier> repo) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult> ListAllSuppliers()
    {
        try
        {
            var suppliers = await repo.ListAllAsync();
            return Ok(new { Success = true, StatusCode = 200, Items = suppliers.Count, Data = suppliers });     
        }
        catch 
        {
            return StatusCode(500, "Sorry, något gick fel");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindSupplier(int id)
    {
        try
        {
            var supplier = await repo.FindByIdAsync(id);
            return Ok(new { Success = true, StatusCode = 200, Items = 1, Data = supplier });
        }
        catch 
        {
            return NotFound("Hittade inget!");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult> FindSupplierByName(string name)
    {
        try
        {
            var supplier = await repo.FindAsync(c => c.SupplierName.ToLower().Trim() == name.ToLower().Trim());
            if(supplier is null) return NotFound("Hittade ingen leveantör");

            return Ok(supplier);
        }
        catch
        {
            return NotFound("Hittade ingen leverantör");
        }
    }

    [HttpPost()]
    public async Task<ActionResult> AddSupplier(Supplier supplier)
    {
        try
        {      
            repo.Add(supplier);
            if (await repo.SaveAllAsync()) return StatusCode(201);

            return StatusCode(500, "Något gick fel när vi skulle spara ny leverantör");
        }
        catch
        {
            return StatusCode(500, "Något gick fel när vi skulle spara ny leverantör");
        }
    }
}

