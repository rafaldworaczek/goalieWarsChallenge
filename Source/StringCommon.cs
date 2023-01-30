using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GlobalsNS
{
    public class StringCommon : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int getIndexOfNOccurence(string str, char strOccur, int n)
        {
            int nOccurence = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == strOccur)
                    nOccurence++;

                if (nOccurence == n)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
