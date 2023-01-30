using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuCustomNS
{
    public class ScheduleMatchesNew
    {
        private List<string> matches;

        public ScheduleMatchesNew(int n, int nRounds)
        {          
            bool dummyN = false;
            int nLoops = 0;
            matches = new List<string>();
            /*when number of teams is odd*/
            if (n % 2 != 0)
            {
                n = n + 1;
                dummyN = true;
            }

            int nCol = n / 2;
            int nRow = 2;
            int[][] data = new int[nRow][];
            for (int i = 0; i < nRow; i++)
            {
                data[i] = new int[nCol];
            }

            for (int row = 0; row < nRow; row++)
            {
                for (int col = 0; col < nCol; col++)
                {
                    if (row == 0)
                    {
                        data[row][col] = col + 1;
                    } else
                    {
                        data[row][col] = n - col;
                    }
                }                   
            }

            for (int m = 1; m <= n - 1; m++)
            {
                for (int col = 0; col < nCol; col++)
                {
                    if (dummyN && (data[0][col] == n ||
                                   data[1][col] == n))
                    {
                        continue;
                    }

                     if (nLoops % 2 == 0)
                        matches.Add(data[0][col].ToString() + "-" + data[1][col].ToString());                 
                     else
                        matches.Add(data[1][col].ToString() + "-" +  data[0][col].ToString());
                }

                /*queue delimeter*/
                matches.Add("0-0");

                /*rotate array*/
                if (nCol == 1)
                    break;

                data[0][1] = data[1][0];
                /*row 1*/
                string res = "1";
                res += " " + data[0][1];
                for (int col = 2; col < nCol; col++)
                {
                    data[0][col] = data[0][col] - 1;
                    if (data[0][col] == 1)
                    {
                        data[0][col] = n;
                    }

                    res += " " + data[0][col];
                }
//                Debug.Log("res1 " + res);


                /*row 2*/
                string res2 = "";
                int tmpCurrVal = data[0][1];
                for (int col = 0; col < nCol; col++)
                {
                    if (--tmpCurrVal == 1)
                        tmpCurrVal = n;

                    data[1][col] = tmpCurrVal;
                    res2 += " " + data[1][col];
                }

                //Debug.Log("res2 " + res2);
                //Debug.Log("------");
                nLoops++;
            }

            List<string> tmpMatches = new List<string>();
            if (nRounds == 2)
            {
                foreach (string match in matches)
                {
                    string[] teamsIdx = match.Split('-');
                    tmpMatches.Add(teamsIdx[1] + "-" + teamsIdx[0]);
                }

                foreach (string match in tmpMatches)
                {
                    matches.Add(match);
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

        public string getScheduleByIndex(int index)
        {
            return matches[index];
        }
    }
}
