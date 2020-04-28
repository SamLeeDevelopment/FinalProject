using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.StringComparison
{
    public class JaccardDistance
    {
        public decimal Distance(String one, String two)
        {

            String[] oneTokens = one.Split(null);
            String[] twoTokens = two.Split(null);



            var commonTokens = from oneQuery in oneTokens
                               join twoQuery in twoTokens on oneQuery equals twoQuery
                               select oneQuery;

            decimal denominator = oneTokens.Count() + twoTokens.Count() - commonTokens.Count();
            decimal numerator = commonTokens.Count();

            decimal distanceD = numerator / denominator;


            distanceD *= 100;


            distanceD = Math.Round(distanceD);

            return distanceD;

        }
    }
}
