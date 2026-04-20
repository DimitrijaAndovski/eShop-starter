using eShop.Entities;
using eShop.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController(IGenericRepository<Customer> repo) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult> ListAllCustomers()
    {
        var customers = await repo.ListAllAsync();
        return Ok(new { Success = true, StatusCode = 200, Items = customers.Count, Data = customers });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindCustomer(int id)
    {
        var customer = await repo.FindByIdAsync(id);
        return Ok(new { Success = true, StatusCode = 200, Items = 1, Data = customer });
    }

    // [HttpPost()]
    // public async Task<ActionResult> AddCustomer(PostCustomerDto model)
    // {
    //     try
    //     {
    //         if(await uow.CustomerRepository.AddCustomer(model))
    //         {
    //             await uow.Complete();
    //             return StatusCode(201);
    //         }

    //     return StatusCode(500,"Ett server ferl inträffade");
    //     }
    //     catch (System.Exception)
    //     {
    //         return StatusCode(500,"Ett server ferl inträffade");
    //     }
    // }

    // [HttpPut("{id}")]
    // public async Task<ActionResult> UpdateCustomer(int id, PutCustomerDto model)
    // {
    //     if(await uow.CustomerRepository.UpdateCustomer(id, model))
    //     {
    //         await uow.Complete();
    //         return NoContent();
    //     }
    //     return NoContent();
    // }

    // [HttpDelete("{id}")]
    // public async Task<ActionResult> DeleteCustomer(int id)
    // {
    //     try
    //     {
    //         if (await uow.CustomerRepository.DeleteCustomer(id))
    //         {
    //             await uow.Complete();
    //             return NoContent();
    //         }
    //         return StatusCode(500,"Ett serverfel inträffade!"); 
    //     }
    //     catch
    //     {
            
    //         return StatusCode(500,"Ett serverfel inträffade!");
    //     }
    // }
}

