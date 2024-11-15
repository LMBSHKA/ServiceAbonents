using Azure;
using Microsoft.AspNetCore.JsonPatch;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Data
{
    public class AbonentRepo : IAbonentRepo
    {
        private readonly AppDbContext _context;
        //private readonly IRemainRepo _remainRepo;
        public AbonentRepo(AppDbContext context)
        {
            //_remainRepo = remainRepo;
            _context = context;
        }

        public void CreateAbonent(Abonent abonent)
        {
            if (abonent == null)
                throw new ArgumentNullException(nameof(abonent));
            //_remainRepo.CreateRemain(abonent.Id);
            _context.Abonents.Add(abonent);
        }

        public Abonent GetAbonentById(int id)
        {
            return _context.Abonents.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Abonent> GetAllAbonents()
        {
            return _context.Abonents.ToList();
        }

        public int GetTariffIdByAbonentId(int id)
        {
            return GetAbonentById(id).TarrifId;
        }

        public void Update(Abonent updateAbonent)
        {
            if (updateAbonent == null)
                throw new ArgumentNullException(nameof(updateAbonent));
            if (GetAbonentById(updateAbonent.Id) == null)
                throw new ArgumentNullException(nameof(updateAbonent.Id));
            _context.Update(updateAbonent);
            _context.SaveChanges();
        } 

        public bool SaveChange()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
