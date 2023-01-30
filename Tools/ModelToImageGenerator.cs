using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using graphicsCommonNS;
using System.IO;
using UnityEditor;

public class ModelToImageGenerator : MonoBehaviour
{
    //format skin_009_05_02_002
    //face_skinColor_beard_tattoo
    // Start is called before the first frame update
    public RenderTexture rt;
    private GraphicsCommon graphics;
    private float realTime = 0;
    public GameObject model;
    public GameObject modelHair;
    private int MAX_FACES = 10;
    private int MAX_SKINCOLOR = 5;
    private int MAX_BEARD = 3;
    private int MAX_TATTOO = 10;
    private int MAX_HAIR = 15;
    int hairIdx = 0;
    bool isDone = false;

    public ParticleSystem flare;
    private string[] hairNames =
    {
"hblack1",
"hblack2",
"hblack3",
"hblack4",
"hblack5",
"hblack6",
"hblack7",
"hblack8",
"hblack9",
"hblack10",
"hblack11",
"hblack12",
"hblack13",
"hblack14",
"hblack15",
"hnohair",
"hblonde1",
"hblonde2",
"hblonde3",
"hblonde4",
"hblonde5",
"hblonde6",
"hred1",
"hred2",
"hred3",
"hred4",
"hred5",
"hred6",
    };


    private string[] filesToCopy =
    {
"skin_0_1_0_0",
"skin_0_2_0_0",
"skin_0_4_0_0",
"skin_0_5_0_0",

"skin_0_1_2_2",
"skin_0_2_2_4",
"skin_0_4_2_5",
"skin_0_5_2_6",

"skin_1_1_0_0",
"skin_1_2_0_0",
"skin_1_4_0_0",
"skin_1_5_0_0",

"skin_1_1_2_7",
"skin_1_2_2_8",
"skin_1_4_2_9",
"skin_1_5_2_10",


"skin_2_1_0_0",
"skin_2_2_0_0",
"skin_2_4_0_0",
"skin_2_5_0_0",

"skin_2_1_2_8",
"skin_2_2_2_9",
"skin_2_4_2_10",
"skin_2_5_2_3",

"skin_3_1_0_0",
"skin_3_2_0_0",
"skin_3_4_0_0",
"skin_3_5_0_0",

"skin_3_1_2_4",
"skin_3_2_2_5",
"skin_3_4_2_6",
"skin_3_5_2_7",

"skin_4_1_0_0",
"skin_4_2_0_0",
"skin_4_4_0_0",
"skin_4_5_0_0",

"skin_4_1_2_8",
"skin_4_2_2_9",
"skin_4_4_2_10",
"skin_4_5_2_4",


"skin_5_1_0_0",
"skin_5_2_0_0",
"skin_5_4_0_0",
"skin_5_5_0_0",

"skin_5_1_2_5",
"skin_5_2_2_6",
"skin_5_4_2_7",
"skin_5_5_2_8",

"skin_6_1_0_0",
"skin_6_3_0_0",
"skin_6_4_0_0",
"skin_6_5_0_0",

"skin_6_1_2_9",
"skin_6_3_2_10",
"skin_6_4_2_3",
"skin_6_5_2_4",

"skin_7_1_0_0",
"skin_7_3_0_0",
"skin_7_4_0_0",
"skin_7_5_0_0",

"skin_7_1_2_5",
"skin_7_3_2_6",
"skin_7_4_2_7",
"skin_7_5_2_8",

"skin_8_1_0_0",
"skin_8_3_0_0",
"skin_8_4_0_0",
"skin_8_5_0_0",

"skin_8_1_2_9",
"skin_8_3_2_10",
"skin_8_4_2_4",
"skin_8_5_2_5",

"skin_9_1_0_0",
"skin_9_3_0_0",
"skin_9_4_0_0",
"skin_9_5_0_0",

"skin_9_1_2_6",
"skin_9_3_2_7",
"skin_9_4_2_8",
"skin_9_5_2_9"
    };

    void Start()
    {
        graphics = new GraphicsCommon();
        flare.Play();

        copyFiles();
        //DirectoryInfo dir = new DirectoryInfo("Assets/Resources/skin");
        //FileInfo[] info = dir.GetFiles("*.*");
        //foreach (FileInfo file in info)
        //{
        //    string newFileName = "";
        //    createPngs(file.FullName);
        //renameFile(file.FullName);
        //}

        //fileName.FullName
        //SaveTexture();
        //toTexture2D(rt);


        toTexture2D(rt, Path.GetFileNameWithoutExtension("powersilverBallx2"));

    }

    // Update is called once per frame
    void Update()
    {
            //setPlayerTexturesSkin
            //    setPlayerTexturesHair

            //toTexture2D(rt);
            if (!isDone)
            {
                StartCoroutine(createPngs(5));
                isDone = true;
            }
    }

    private void copyFiles()
    {
        /*for (int i = 0; i < filesToCopy.Length; i++)
        {
            FileUtil.CopyFileOrDirectory(
     "C:/Users/dwo/Desktop/Resources_backup/Resourcec_20_04_playersMaterial_lcoals/playerMaterials/skins/" + filesToCopy[i] + ".mat.meta",
     "C:/Users/dwo/playerMaterialNew/" + filesToCopy[i] + ".mat.meta");

            FileUtil.CopyFileOrDirectory(
    "C:/Users/dwo/Desktop/Resources_backup/Resourcec_20_04_playersMaterial_lcoals/playerMaterials/skins/" + filesToCopy[i] + ".mat",
    "C:/Users/dwo/playerMaterialNew/" + filesToCopy[i] + ".mat");
        }*/

    }

    IEnumerator createPngs(float delay)
    {
        yield return new WaitForSeconds(delay);

        toTexture2D(rt, Path.GetFileNameWithoutExtension("powersilverBallx2"));

        yield break;

        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/playerMaterials/skins");
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo file in info)
        {
            string stringFileName = file.FullName;

            //if (!stringFileName.Split('_')[4].Split('.')[0].Equals("0"))
            //{
            //    print("FILETAKEN NO " + stringFileName + " char " +
            //        stringFileName.Split('_')[4].Split('.')[0]);
            //}

            if (stringFileName.Contains("meta"))
                //!stringFileName.Split('_')[4].Split('.')[0].Equals("0"))
                continue;

            print("FILETAKEN YES## " + stringFileName);

            print("#DBGXAX filename " + stringFileName + " Path.GetFileName(stringFileName) " +
                Path.GetFileNameWithoutExtension(stringFileName));

            for (int hairIdx = 0; hairIdx < hairNames.Length; hairIdx++)
            {
                string hairName = hairNames[hairIdx];
                //"hblack" + hairIdx.ToString();
                print("#DBGHAIR1 HAIRNAMEGEN " + hairName + " skin path " +
                    Path.GetFileNameWithoutExtension(stringFileName));

                graphics.setPlayerTexturesSkin(model, Path.GetFileNameWithoutExtension(stringFileName));
                graphics.setPlayerTexturesHair(modelHair, hairName);

                string fileName = Path.GetFileNameWithoutExtension(stringFileName);
                string[] fileNameSplit = fileName.Split('_');
                string outFileName = "";

                //f0_s1_b0_t0_hblack5
                for (int i = 1; i < fileNameSplit.Length; i++)
                {
                    if (i == 1)
                        outFileName += "f";
                    if (i == 2)
                        outFileName += "_s";

                    if (i == 3)
                        outFileName += "_b";
                    if (i == 4)
                        outFileName += "_t";
                    outFileName += fileNameSplit[i];
                }

                outFileName += "_" + hairName;
                print("#DBGHAIR1 RESFILENAME " + outFileName);

                //toTexture2D(rt, Path.GetFileNameWithoutExtension(stringFileName) + "_" + 
                //   hairName.Replace("_", ""));
                yield return new WaitForSeconds(0.3f);

                toTexture2D(rt, Path.GetFileNameWithoutExtension(outFileName));

                //hairIdx++;
                //if (hairIdx >= hairNames.Length)
                //    hairIdx = 0;
                //toTexture2D(rt, fileName);
            }
        }
    }



        //for (int face = 0; face < MAX_FACES; face++)
        //    for (int skin = 1; skin <= MAX_SKINCOLOR; skin++)
        //        for (int beard = 0; beard < MAX_BEARD; beard++)
        //            for (int tattoo = 1; tattoo <= MAX_TATTOO; tattoo++)
        //            {
        //string skinName =
        //    "skin_" + face + "_" + skin + "_" + beard + "_" +

        /*graphics.setPlayerTexturesSkin(model, Path.GetFileName(fileName));
                        graphics.setPlayerTexturesHair(modelHair, "hair_" + hairIdx.ToString());
                        if (hairIdx >= MAX_HAIR)
                            hairIdx = 1;

                        toTexture2D(rt, fileName);
                        yield return new WaitForSeconds(time);
                    }*/
                    
    private void renameFile(string fileName)
    {
        string[] partsNames = fileName.Split('_');
        string directory = Path.GetDirectoryName(fileName);
        string face = (int.Parse(partsNames[1])).ToString();
        string skinColor = (int.Parse(partsNames[2])).ToString();
        string beard = (int.Parse(partsNames[3])).ToString();

        string[] extensionNames = partsNames[4].Split('.');
        string tattoo = "";

        print("PARTSNAME " + partsNames[4] + " extenNames " + extensionNames.Length 
            + " fileName " + fileName + " partsNamesLen " + partsNames.Length + " partsNames4 " +
            partsNames[4] + " extension " + Path.GetExtension(fileName));

        if (extensionNames.Length > 1)
        {
            tattoo += (int.Parse(extensionNames[0]).ToString()) + "." + extensionNames[1];
            if (extensionNames.Length > 2)
                tattoo += "." + extensionNames[2];
        } else
        {
            tattoo = (int.Parse(partsNames[4])).ToString();
        }

        //if (counter == 0)
        //{
        string nameAfter = directory + "\\" + "skin_" + face + "_" + skinColor + "_" + beard + "_" + tattoo;

        //string nameAfter = directory + "\\" + "skin_" + face + "_" + skinColor + "_" + beard + "_" + tattoo;


        System.IO.File.Move(fileName, nameAfter);
        //    counter = 1;
        //}

        print("#DBGAFTER Filename " + fileName + " After " + nameAfter);

    }

 
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        //toTexture2D(rt);
        isDone = true;
        // Code to execute after the delay
    }

    // Use this for initialization
    //public void SaveTexture()
    //{
    //    byte[] bytes = toTexture2D(rt).EncodeToPNG();
    //}

    Texture2D toTexture2D(RenderTexture rTex, string fileName)
    {  
        var oldRT = RenderTexture.active;
        var tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        System.IO.File.WriteAllBytes("C:/Users/dwo/playerCards/" + fileName + ".png", tex.EncodeToPNG());
        RenderTexture.active = oldRT;
        return null;
    }
}
