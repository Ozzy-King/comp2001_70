namespace WebApp2
{
    public class ipLogger
    {
        public static Dictionary<string, int> ipCount = new Dictionary<string, int>();
        public static int limitPerMin = 25;

        //adds ip if needed and then incrment by one
        static void incrementIP(string ip) {
            ipCount[ip]++;
        }
        //gets the ip count -1 if ip dosnet exists
        static int getIpCount(string ip) {
            if (ipCount.ContainsKey(ip)) {
                return ipCount[ip];
            }
            return -1;
        }

        //gets and checks api count limit, returns true if over the minlimit incremnts if not over the limit.
        public static bool check(string ip) {
            if (!ipCount.ContainsKey(ip)) {
                ipCount.Add(ip, 0);
            }
            if (getIpCount(ip) == limitPerMin) {
                return true;
            } else if (getIpCount(ip) == 0) { //if this is the first time check is used when the value is zero then start a thread timer to reset value after one minute
                new Thread(()=>ipcheckerReserter(ip)).Start();
            }
            incrementIP(ip);
            return false;
        }

        static void ipcheckerReserter(String ip) {
            Thread.Sleep(60000);
            ipCount[ip] = 0;
        }

    }
}
