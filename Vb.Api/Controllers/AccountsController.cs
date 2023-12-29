using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly VbDbContext dbContext;

    public AccountsController(VbDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public async Task<List<Account>> Get()
    {
        return await dbContext.Set<Account>().ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<Account> Get(int id)
    {
        var account = await dbContext.Set<Account>()
            .Include(x => x.Customer)
            .Include(x => x.AccountTransactions)
            .Include(x => x.EftTransactions)
            .Where(x => x.Id == id).FirstOrDefaultAsync();

        return account;
    }

    [HttpPost]
    public async Task Post([FromBody] Account account)
    {
        await dbContext.Set<Account>().AddAsync(account);
        await dbContext.SaveChangesAsync();
    }

    [HttpPut("{id}")]
    public async Task Put(int id, [FromBody] Account account)
    {
        var fromdb = await dbContext.Set<Account>().Where(x => x.Id == id).FirstOrDefaultAsync();
        dbContext.Accounts.Update(account);

        await dbContext.SaveChangesAsync();
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        var fromdb = await dbContext.Set<Account>().Where(x => x.Id == id).FirstOrDefaultAsync();
        fromdb.IsActive = false;
        await dbContext.SaveChangesAsync();
    }
}