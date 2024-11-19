using ApiNetCore8.Models;

namespace ApiNetCore8.Repositores
{
    public interface IDashBoardRepository
    {
        public Task<DashBoardModel> GetDashBoardInDayAsync();
        public Task<DashBoardModel> GetDashBoardAllAsync();
    }
}
