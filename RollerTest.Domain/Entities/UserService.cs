using System.ComponentModel.DataAnnotations;

namespace RollerTest.Domain.Entities
{
    public class MoterService
        {
        [Key]
        public int Id { get; set; }
            public bool MoterWatcher { get; set; }
            public bool IsMotoRunning { get; set; }
            public int MotoREV { get; set; }
        }
        public class ForcerService
        {
        [Key]
        public int Id { get; set; }
        public bool ForcerWatcher { get; set; }

        }
        public class TimerService
        {
        [Key]
        public int Id { get; set; }
        public bool TimeWriter { get; set; }
            public bool TimeTick { get; set; }


        }
}
