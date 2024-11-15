using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Data
{
    public class RemainRepo : IRemainRepo
    {
        private readonly AppDbContext _context;

        public RemainRepo(AppDbContext context)
        {
            _context = context;
        }


        public void CreateRemain(Remain newRemain)
        {
            if (newRemain.ClientId == 0)
                throw new ArgumentException(nameof(newRemain.ClientId));
            if (newRemain == null)
                throw new NullReferenceException(nameof(newRemain));
            _context.Remains.Add(newRemain);
        }

        public IEnumerable<Remain> GetAllRemains()
        {
            return _context.Remains.ToList();
        }

        public Remain GetRemainByAbonentId(int id)
        {
            return _context.Remains.FirstOrDefault(x => x.ClientId == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void Update(Remain remain)
        {
            if (remain == null)
                throw new NullReferenceException();
            _context.Update(remain);
            _context.SaveChanges();
        }
    }
}
