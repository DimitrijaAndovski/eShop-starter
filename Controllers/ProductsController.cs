using AutoMapper;
using eShop.Data;
using eShop.DTOs;
using eShop.DTOs.Products;
using eShop.Entities;
using eShop.Interfaces;
using eShop.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace eShop.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController(IGenericRepository<Product> repo, IMapper mapper) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult> ListAllProducts(string? brand, string? sort)
    {
        try
        {
            var spec = new ProductSpecification(itemnumber: null,brand, sort: sort);
            var products = await repo.ListAsync(spec);
            var productsDto = mapper.Map<IList<GetProductsDto>>(products);

            return Ok(new {Success = true, StatusCode = 200, Items = products.Count, Data = productsDto});
        }
        catch
        {
            return StatusCode(500, "Server fel inträffade");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindProduct(int id)
    {
        try
        {
            var product = await repo.FindByIdAsync(id);
            return Ok(new { Success = true, StatusCode = 200, Items = 1, Data = product });
        }
        catch
        {  
            return StatusCode(500, "Server fel inträffade, vi kan inte hitta produkten");

        }
    
    }

    [HttpPost()]
    public async Task<ActionResult> AddProduct(PostProductDto model)
    {
        try
        {   
            var product = mapper.Map<Product>(model);

            repo.Add(product);

            if(await repo.SaveAllAsync())
            {
                return StatusCode(201);
            }
           
            return StatusCode(500, "Något server fel inträffade!");
        }
        catch
        {  
            return StatusCode(500, "Något server fel inträffade!");
        }
    }

    [HttpGet("product/{itemNumber}")]
    public async Task<ActionResult> FindProduct(string itemNumber)
    {

        try
        {
            var spec = new ProductSpecification(itemNumber, brand: null, sort: null);
            var product = await repo.FindAsync(spec);

            if (product is null) return NotFound();

            return Ok(new { Success = true, StatusCode = 200, Items = 1, Data = product });
        }
        catch
        {
            return StatusCode(500, "Server fel inträffade, vi kan inte hitta produkten");
        }
    }

    

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        try
        {
            product.Id = id;
            
            repo.Update(product);
            if(await repo.SaveAllAsync())
            {
                return NoContent();
            }


            return StatusCode(500, "Något server fel inträffade!");
            
        }
        catch
        {
            return StatusCode(500, "Något server fel inträffade!");

        }

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveProduct(int id)
    {
        try
        {
            var product = await repo.FindByIdAsync(id);
            repo.Delete(product);
            if(product is null) return BadRequest("hittade inte product!!");

            repo.Delete(product);

            if (await repo.SaveAllAsync()) return NoContent();

            return StatusCode(500, "Ett server fel inträffade!");

        }
        catch
        {
            return StatusCode(500, "Ett server fel inträffade!");
        }

    }
}

