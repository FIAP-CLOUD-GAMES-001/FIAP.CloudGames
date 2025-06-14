using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.infrastructure.Data;
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{

}