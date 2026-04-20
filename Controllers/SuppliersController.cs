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

    // [HttpPost()]
    // public async Task<ActionResult> AddSupplier(PostSupplierDto supplier)
    // {
    //     try
    //     {
    //         var id = await uow.SupplierRepository.AddSupplier(supplier);

    //         if(await uow.SupplierRepository.AddSupplier(supplier) > 0)
    //         {
    //             return CreatedAtAction(nameof(FindSupplier), new { id }, supplier);
    //         }
    //         else
    //         {
    //             return BadRequest("Vi saknar information angående fel");
    //         }
            
    //     }
    //     catch
    //     {
    //         return StatusCode(500, "Något gick fel när vi skulle spara ny leverantör");
    //     }
    // }
}

