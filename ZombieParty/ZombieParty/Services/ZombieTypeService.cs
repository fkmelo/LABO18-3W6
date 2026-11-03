using Microsoft.EntityFrameworkCore;
using ZombieParty.Models;
using ZombieParty.Models.Data;

namespace ZombieParty.Services
{
    public class ZombieTypeService : ServiceBaseAsync<ZombieType>, IZombieTypeService
    {
        /* Ici, on passe le dbContext au constructeur de la classe parente ServiceBaseAsync avec base.
         * C’est nécessaire, parce que la classe ZombieTypeService hérite de ServiceBaseAsync qui a une le context en paramètre. */
        public ZombieTypeService(ZombiePartyDbContext dbContext) : base(dbContext)
        {

        }

        public bool HasAssociatedZombies(int id)
        {
            return  _dbContext.Zombies.Any(x => x.ZombieTypeId == id);
        }

        public override async Task DeleteAsync(int id)
        {
            var zombieType = await this.GetByIdAsync(id);
            if (zombieType == null)
                return;
            if (HasAssociatedZombies(id)) // Si zombies associés, on le rend indisponible.
            {
                zombieType.IsDisponible = false;
                await this.EditAsync(zombieType);
            }
            else                          // Sinon on supprime le ZombieType
            {
                await base.DeleteAsync(id); /* base permet d’utiliser le comportement original de DeleteAsync implémenté dans la classe parente. */
            }
        }

    }
}
