using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuCustomNS
{
    public class ScheduleMatches
    {
        private List<string> matches;

        public ScheduleMatches(int n)
        {
            bool[][] data = new bool[n + 1][];
            for (int i = 0; i < n + 1; i++)
            {
                data[i] = new bool[n + 1];
            }

            bool[] marked = new bool[n + 1];
            int countMatches = 0;
            int nMatches = (n * (n - 1)) / 2;
            matches = new List<string>();

            for (int row = 1; row <= n; row++)
            {
                for (int col = 1; col <= n; col++)
                    if (row == col)
                        data[row][col] = true;
                    else
                        data[row][col] = false;
            }

            int teamA, teamB;
            bool found = false;

            while (countMatches < nMatches)
            {
                for (int i = 1; i <= n; i++)
                    marked[i] = false;

                while (true)
                {
                    found = false;
                    for (teamA = 1; teamA <= n; teamA++)
                    {
                        if (marked[teamA] == false)
                        {
                            found = true;
                            marked[teamA] = true;
                            break;
                        }
                    }

                    if (!found) break;

                    for (int col = 1; col <= n; col++)
                    {
                        if (data[teamA][col] || marked[col]) continue;

                        teamB = col;
                        string match = teamA + "-" + teamB;
                        data[teamA][teamB] = data[teamB][teamA] = true;
                        marked[teamB] = true;
                        matches.Add(match);
                        countMatches++;
                        break;
                    }
                }
            }
        }

        public List<string> getListOfMatches()
        {
            return matches;
        }

        public IEnumerable<string> getSchedule()
        {
            foreach (var items in matches)
            {
                // Returning the element after every iteration 
                yield return items;
            }
        }
    }
}
