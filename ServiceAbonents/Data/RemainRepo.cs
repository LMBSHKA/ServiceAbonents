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
            if (newRemain.ClientId.Equals(null) || newRemain.ClientId.Equals(Guid.Empty))
                throw new ArgumentException(nameof(newRemain.ClientId));

            if (newRemain == null)
                throw new NullReferenceException(nameof(newRemain));

            _context.Remains.Add(newRemain);
            _context.SaveChanges();
        }

        public IEnumerable<Remain> GetAllRemains()
        {
            return _context.Remains.ToList();
        }

        public Remain GetRemainByAbonentId(Guid id)
        {
            return _context.Remains.FirstOrDefault(x => x.ClientId.Equals(id));
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void Update(Guid clientId, RemainUpdateDto updateRemain)
        {
            var remain = GetRemainByAbonentId(clientId);

            if (remain == null)
                throw new NullReferenceException();

            if (updateRemain.RemainGb > 0)
                remain.RemainGb = (decimal)updateRemain.RemainGb;

            if (updateRemain.RemainMin > 0)
                remain.RemainMin = (decimal)updateRemain.RemainMin;

            if (updateRemain.RemainSMS > 0)
                remain.RemainSMS = (decimal)updateRemain.RemainSMS;

            if (updateRemain.UnlimVideo != null)
                remain.UnlimVideo = (bool)updateRemain.UnlimVideo;

            if (updateRemain.UnlimSocials != null)
                remain.UnlimSocials = (bool)updateRemain.UnlimSocials;

            if (updateRemain.UnlimMusic != null )
                remain.UnlimMusic = (bool)updateRemain.UnlimMusic;

            if (updateRemain.LongDistanceCall != null)
                remain.LongDistanceCall = (bool)updateRemain.LongDistanceCall;

            _context.Update(remain);
            _context.SaveChanges();
        }
    }
}
