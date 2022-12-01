using Advent_of_Code.Challenge_Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code.Reader
{
    internal interface ChallengeReader
    {
        public ChallengeSolution ReadChallenge();
    }
}
