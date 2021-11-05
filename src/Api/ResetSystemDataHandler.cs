using Data;

namespace Api
{
    public class ResetSystemDataHandler
    {
        private readonly CampaignDbContext _context;

        public ResetSystemDataHandler(CampaignDbContext context)
        {
            _context = context;
        }

        public void ResetDb()
        {
            DataSeeding.ResetDb(_context);
        }
    }
}
