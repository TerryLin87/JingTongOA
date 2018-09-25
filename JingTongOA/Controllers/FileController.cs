using JianTongBLL;
using JianTongFun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JingTongOA.Controllers
{
    public class FileController : BaseController
    {
        private static object fileLock = new object();
        public JsonResult FileUpload(string workId, string fileType)
        {
            lock (fileLock)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "UpLoad\\WorkFile\\" + workId + "\\" + fileType;
                var files = Request.Files;

                Dictionary<string, string> aaa = new Dictionary<string, string>();
                if (files != null)
                {
                    List<WorkFile> workFileList = new List<WorkFile>();
                    int typeId = OperationFunction.CreateInstance.StringConvertToInt(fileType, 0);
                    foreach (String fileKey in files)
                    {
                        HttpPostedFileBase fileBase = files.Get(fileKey);

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        fileBase.SaveAs(string.Format("{0}\\{1}", filePath, fileBase.FileName));
                        WorkFile workFile = new WorkFile();
                        workFile.fileName = fileBase.FileName;
                        workFile.workId = workId;
                        workFile.fileUserId = CurrentUser.Id;
                        workFile.fileTypeId = typeId;
                        workFileList.Add(workFile);

                        aaa.Add("fileName", fileBase.FileName);
                        aaa.Add("fileId", workFile.Id);
                    }
                    EntityFun.CreateInstance.UploadWorkFile(workId, CurrentUser.Id, workFileList);
                }
                return Json(aaa);
            }
        }

        public JsonResult DeleteFile(string workFileId)
        {
            if (string.IsNullOrWhiteSpace(workFileId))
            {
                return Json("");
            }

            WorkFile workFile = EntityFun.CreateInstance.GetTByStringId<WorkFile>(workFileId);
            if (workFile == null)
            {
                return Json("");
            }
            string path = AppDomain.CurrentDomain.BaseDirectory + "UpLoad\\WorkFile\\" + workFile.workId + "\\" + workFile.fileTypeId;
            if (System.IO.File.Exists(string.Format("{0}\\{1}", path, workFile.fileName)))
            {
                System.IO.File.Delete(string.Format("{0}\\{1}", path, workFile.fileName));
            }
            EntityFun.CreateInstance.DeleteTByStringIds<WorkFile>(new string[] { workFile.Id });
            return Json("");
        }

        public FileStreamResult DownLoadFiles(string id, string fileType, string fileName)
        {
            try
            {

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "UpLoad\\WorkFile\\" + id + "\\" + fileType + "\\" + fileName;
                return File(new FileStream(filePath, FileMode.Open), "application/octet-stream", fileName);
            }
            catch (Exception ce)
            {
            }
            return null;
        }
    }
}