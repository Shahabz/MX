﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WayLoader:Singleton<WayLoader> {
    Dictionary<string, List<WayPoint>> data = new Dictionary<string, List<WayPoint>>();
    public List<WayPoint> Load(string file)
    {
        string key = file + ".wp";
        if (data.ContainsKey(key))
            return data[key];
        List<WayPoint> wp = new List<WayPoint>();
        TextAsset asset = Resources.Load<TextAsset>(key);
        if (asset != null)
        {
            MemoryStream ms = new MemoryStream(asset.bytes);
            StreamReader read = new StreamReader(ms);
            int wayCount = 0;
            WayPoint wpeach = null;
            while (!read.EndOfStream)
            {
                string line = read.ReadLine();
                string[] eachline = line.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (eachline[0] == "WayPoints")
                {
                    wayCount = int.Parse(eachline[1]);
                }
                else if (eachline[0] == "Pos")
                {
                    if (wpeach != null)
                        wp.Add(wpeach);
                    wpeach = new WayPoint();
                    wpeach.pos = new Vector3(float.Parse(eachline[1]), float.Parse(eachline[3]), float.Parse(eachline[2]));
                }
                else if (eachline[0] == "Size")
                {
                    wpeach.size = int.Parse(eachline[1]);
                }
                else if (eachline[0] == "Link")
                {
                    wpeach.link = new Dictionary<int, WayLength>();
                }
                else
                {
                    WayLength wayl = new WayLength();
                    wayl.length = float.Parse(eachline[2]);
                    wayl.mode = int.Parse(eachline[1]);
                    wpeach.link.Add(int.Parse(eachline[0]), wayl);
                }
            }
            if (!wp.Contains(wpeach))
                wp.Add(wpeach);
        }
        data.Add(key, wp);
        return wp;
    }
}