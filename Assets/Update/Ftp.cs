﻿using System; 
using System.Collections.Generic; 
using System.Text; 
using System.Net; 
using System.IO;
using UnityEngine; 

public static class FtpLog
{
	private static void CompressFileLZMA(string inFile, string outFile)
	{
		SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
		FileStream input = new FileStream(inFile, FileMode.Open);
		FileStream output = new FileStream(outFile, FileMode.Create);
		
		// Write the encoder properties
		coder.WriteCoderProperties(output);
		
		// Write the decompressed file size.
		for (int i = 0; i < 8; i++)
			output.WriteByte((Byte)(input.Length >> (8 * i)));

		coder.Code(input, output, input.Length, -1, null);
		output.Flush();
		output.Close();
		input.Close();
	}
    static List<string> logFile = new List<string>();
    static System.Threading.Thread uploadThread;
    public static void Uninit()
    {
        if (uploadThread != null)
            uploadThread.Abort();
        logFile.Clear();
    }
    public static string UUID;
    public static void UploadStart()
    {
        FileStream fs = null;
        UUID = SystemInfo.deviceUniqueIdentifier;
        string strfile = Application.persistentDataPath + "/" + Application.platform + "_debug.log";
        string strfile2 = Application.persistentDataPath + "/" + Application.platform + "_error.log";
        if (File.Exists(strfile))
        {
            fs = File.Open(strfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (fs == null)
                return;
            if (fs.Length == 0)
            {
                fs.Close();
                return;
            }
        }
        else
            return;
        if (fs != null)
            fs.Close();

        DateTime time = DateTime.UtcNow;
        logFile.Clear();
        logFile.Add(strfile);
        logFile.Add(strfile2);

        if (uploadThread == null)
        {
            uploadThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(FtpLog.UploadLog));
            uploadThread.Start();
        }
    }

    static void UploadLog(object param)
    {
        int fileCount = 0;
        for (int i = 0; i < logFile.Count; i++)
        {
            if (!File.Exists(logFile[i]))
                continue;
            fileCount++;
            FileStream fs = File.Open(logFile[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FileInfo finfo = new FileInfo(logFile[i]);
            FtpUpload(fs, finfo.Name);
            if (fs != null)
                fs.Close();
            //File.Delete(logFile[i]);
        }
        LocalMsg msg = new LocalMsg();
        msg.Message = (int)LocalMsgType.SendFTPLogComplete;
        msg.Result = 1;
        msg.Param = fileCount;
        ProtoHandler.PostMessage(msg);
        uploadThread = null;
    }

	static void FtpUpload(FileStream fs, string name)
	{
		FtpWebRequest ftp;
		ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://www.idevgame.com/" + UUID + "_" + System.DateTime.Now.ToString("dd-MM-yy") + "_" + name));
		ftp.Credentials = new NetworkCredential("winson", "xuwen1013");
		ftp.UsePassive = true;
		ftp.ContentLength = fs.Length;
		ftp.KeepAlive = true; 
		ftp.Method = WebRequestMethods.Ftp.UploadFile;
		ftp.UseBinary = true;
        ftp.Timeout = 10000;
		int buffLength = 2048; 
		byte[] buff = new byte[buffLength];
        System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
        int contentLen;
		Stream sw = null;
        try
        {
            sw = ftp.GetRequestStream();
			contentLen = fs.Read(buff, 0, buffLength); 
			while (contentLen != 0) 
			{
				sw.Write(buff, 0, contentLen); 
				contentLen = fs.Read(buff, 0, buffLength); 
			}
			sw.Close();
        } 
		catch (Exception e) 
		{
            UnityEngine.Debug.LogError("upload log file exception:" + e.Message);
		}
		finally
		{
			if (sw != null)
				sw.Close();
        }
	}
} 